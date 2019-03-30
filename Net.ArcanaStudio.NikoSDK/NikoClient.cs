using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Net.ArcanaStudio.NikoSDK.Converters;
using Net.ArcanaStudio.NikoSDK.Interfaces;
using Net.ArcanaStudio.NikoSDK.Model.Commands;
using Net.ArcanaStudio.NikoSDK.Models;
using Newtonsoft.Json;

namespace Net.ArcanaStudio.NikoSDK
{
    public class NikoClient
    {

        private readonly string _ipaddress;
        private TcpClient _tcpClient;
        private readonly Observable<string> _observableResponses;
        private readonly Observable<string> _observableEvents;
        private IDisposable _eventObserverSubscriber;
        public event EventHandler<IEvent> OnValueChanged;

        public bool IsConnected => _tcpClient != null && _tcpClient.Connected;

        public IPAddress IpAddress => IPAddress.Parse(_ipaddress);

        public static Task<NikoClient> AutoDetect()
        {
            var tcs = new TaskCompletionSource<NikoClient>();

            var interfaces = NetworkInterface.GetAllNetworkInterfaces().Where(d =>
                (d.OperationalStatus == OperationalStatus.Up) &&
                (d.NetworkInterfaceType != NetworkInterfaceType.Loopback) &&
                (d.NetworkInterfaceType != NetworkInterfaceType.Tunnel)).ToList();

             
            if (!interfaces.Any())
                throw new Exception("No valid network");

            var s = new UdpClient(1000);
            var sendbuf = Encoding.ASCII.GetBytes("D");
            var found = false;
            var nikoip = string.Empty;

            var ep = new IPEndPoint(IPAddress.Broadcast, 10000);

            s.Send(sendbuf, sendbuf.Length, ep);

            try
            {
                IPEndPoint rep = null;
                var received = s.Receive(ref rep);
                nikoip = rep.Address.ToString();

                tcs.TrySetResult(new NikoClient(nikoip));
                found = true;
                //break;
            }
            catch (Exception)
            {

            }
            finally
            {
                s.Close();
            }

            if (!found)
                tcs.TrySetException(new Exception("No Niko controller found"));

            return tcs.Task;
        }

        public NikoClient(string ipaddress)
        {
            if (!IsValidIP(ipaddress))
                throw new ArgumentException(nameof(ipaddress));

            _ipaddress = ipaddress;

            _observableResponses = new Observable<string>();
            _observableEvents = new Observable<string>();

            _eventObserverSubscriber = _observableEvents.Subscribe(new ActionObserver<string>(SendEvent, (s) => { }));
        }

        private bool IsValidIP(string ipaddress)
        {
            if (IPAddress.TryParse(ipaddress, out IPAddress theaddress))
            {
                return theaddress.ToString().Equals(ipaddress);
            }

            return false;
        }


        public Task<INikoResponse<ISystemInfo>> GetSystemInfo()
        {
            var tcs = new TaskCompletionSource<INikoResponse<ISystemInfo>>();
            SendCommand<SystemInfo>(new GetSystemInfoCommand(), new SystemInfoConverter()).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerException);
                else
                    tcs.TrySetResult(t.Result);
            });

            return tcs.Task;
        }

        public Task<INikoResponse<IBaseResponse>> StartEvents()
        {
            var tcs = new TaskCompletionSource<INikoResponse<IBaseResponse>>();
            SendCommand<BaseResponse>(new StartEventsCommand()).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerException);
                else
                    tcs.TrySetResult(t.Result);
            });

            return tcs.Task;
        }

        public Task<INikoResponse<ILocations>> GetLocations()
        {
            var tcs = new TaskCompletionSource<INikoResponse<ILocations>>();
            SendCommand<ILocations>(new GetLocationsCommand(), new LocationsConcreteTypeConverter()).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerException);
                else
                    tcs.TrySetResult(t.Result);
            });

            return tcs.Task;
        }

        public Task<INikoResponse<IActions>> GetActions()
        {
            var tcs = new TaskCompletionSource<INikoResponse<IActions>>();
            SendCommand<IActions>(new GetActionsCommand(), new ActionConcreteTypeConverter()).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerException);
                else
                    tcs.TrySetResult(t.Result);
            });

            return tcs.Task;
        }

        public Task<INikoResponse<IBaseResponse>> ExecuteCommand(int id, int value)
        {
            var tcs = new TaskCompletionSource<INikoResponse<IBaseResponse>>();
            SendCommand<IBaseResponse>(new ExecuteCommand(id,value), new ConcreteTypeConverter<IBaseResponse,BaseResponse>()).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerException);
                else
                    tcs.TrySetResult(t.Result);
            });

            return tcs.Task;
        }

        public Task<NikoResponse<T>> SendCommand<T>(NikoCommandBase command, params JsonConverter[] converters)
        {
            var tcs = new TaskCompletionSource<NikoResponse<T>>();
            var bytes = Encoding.ASCII.GetBytes(Serialize(command));
#if DEBUG
            Debug.WriteLine("Message sent : " + Serialize(command));
#endif
            IDisposable observer = null;

            observer = _observableResponses.Subscribe(new ActionObserver<string>(s =>
            {
                try
                {
                    //Check if it is response from same command
                    if (s.IndexOf(command.CommandName,StringComparison.OrdinalIgnoreCase)>0)
                    {
                        tcs.TrySetResult(Deserialize<T>(s, converters));
                    }
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                }
                finally
                {
                    observer.Dispose();
                }
            }, e =>
            {
                tcs.SetException(e);
                observer.Dispose();
            }));

            _tcpClient.GetStream().WriteAsync(bytes, 0, bytes.Length);

            return tcs.Task;
        }

        private string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data) + "\n";
        }

        private NikoResponse<T> Deserialize<T>(string s,params JsonConverter[] converters)
        {
            return JsonConvert.DeserializeObject<NikoResponse<T>>(s, converters);
        }

        public void StartClient()
        {
            if (IsConnected)
                return;

            _tcpClient = new TcpClient(_ipaddress, 8000);


            _tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.KeepAlive,true);

#pragma warning disable 4014
            ReadData();
#pragma warning restore 4014
        }

        private async Task ReadData()
        {
            bool listen = true;
            var stream = _tcpClient.GetStream();
            var buffer = new byte[10000];
            int index = 0;

            while (listen)
            {
                var br = await stream.ReadAsync(buffer, index, buffer.Length - index);

                if (br == 0)
                    continue;

                index += br;

                if (buffer[index - 2] == '\r' || buffer[index - 1] == '\n')
                {
                    var datastring = Encoding.ASCII.GetString(buffer, 0, index);
                    foreach (var s in datastring.Split(new[] {"\n\r"},StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (s.StartsWith("{\"event\""))
                            _observableEvents.Add(s);
                        else
                            _observableResponses.Add(s);
                    }
                    index = 0;
#if DEBUG
                    Debug.WriteLine("Message received : " + datastring);
#endif
                }  
            }
        }

        public void StopClient()
        {
            if (IsConnected)
                _tcpClient.Close();
        }

        private void SendEvent(string eventdata)
        {
            var eventlist = eventdata.Split('\n').Where(d => !string.IsNullOrEmpty(d));

            foreach (var e in eventlist)
            {
                var theevent = JsonConvert.DeserializeObject<EventImp>(e, new ConcreteTypeConverter<IEventItem, EventItem>());
                OnValueChanged?.Invoke(this, theevent);
            }

        }

   
    }
}
