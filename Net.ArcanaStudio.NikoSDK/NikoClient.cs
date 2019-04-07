using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Net.ArcanaStudio.NikoSDK.Converters;
using Net.ArcanaStudio.NikoSDK.Interfaces;
using Net.ArcanaStudio.NikoSDK.Model;
using Net.ArcanaStudio.NikoSDK.Model.Commands;
using Net.ArcanaStudio.NikoSDK.Models;
using Net.ArcanaStudio.NikoSDK.Shared.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Net.ArcanaStudio.NikoSDK
{
    public class NikoClient
    {
        private readonly ITcpClient _tcpClient;
        private readonly Observable<JObject> _observableResponses = new Observable<JObject>();
        private bool _readFromSocket;

        public event EventHandler<IEvent> OnValueChanged;

        public bool IsConnected => _tcpClient.IsConnected;

        public IPAddress IpAddress => _tcpClient.IpAddress;

        [ExcludeFromCodeCoverage]
        public static Task<NikoClient> AutoDetect(int localport = 1000)
        {
            var tcs = new TaskCompletionSource<NikoClient>();

            var interfaces = NetworkInterface.GetAllNetworkInterfaces().Where(d =>
                (d.OperationalStatus == OperationalStatus.Up) &&
                (d.NetworkInterfaceType != NetworkInterfaceType.Loopback) &&
                (d.NetworkInterfaceType != NetworkInterfaceType.Tunnel)).ToList();

             
            if (!interfaces.Any())
                throw new NikoClientException("No valid network");

            var s = new UdpClient(localport);
            var sendbuf = Encoding.ASCII.GetBytes("D");
            var found = false;

            var ep = new IPEndPoint(IPAddress.Broadcast, 10000);

            s.Send(sendbuf, sendbuf.Length, ep);

            try
            {
                IPEndPoint rep = null;
                s.Receive(ref rep);
                var nikoip = rep.Address.ToString();

                tcs.TrySetResult(new NikoClient(nikoip));
                found = true;
            }
            catch (Exception)
            {
                // ignored
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

            _tcpClient = new NikoTcpClient(ipaddress);
        }

        public NikoClient(ITcpClient client)
        {
            _tcpClient = client ?? throw new ArgumentNullException(nameof(client));
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
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    // ReSharper disable once PossibleNullReferenceException
                    tcs.TrySetException(t.Exception.InnerException);
                }
                else
                    tcs.TrySetResult(t.Result);
            });

            return tcs.Task;
        }

        public Task<IBaseResponse> StartEvents()
        {
            var tcs = new TaskCompletionSource<IBaseResponse>();
            SendCommand<ErrorImp>(new StartEventsCommand(), new BaseResponseConverter()).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    // ReSharper disable once PossibleNullReferenceException
                    tcs.TrySetException(t.Exception.InnerException);
                }
                else
                {
                    var br = new BaseResponse(t.Result.Data.Error) { Command = t.Result.Command};
                    tcs.TrySetResult(br);
                }
            });

            return tcs.Task;
        }

        public Task<INikoResponse<IReadOnlyList<ILocation>>> GetLocations()
        {
            var tcs = new TaskCompletionSource<INikoResponse<IReadOnlyList<ILocation>>>();
            SendCommand<IReadOnlyList<ILocation>>(new GetLocationsCommand(), new LocationsConverter()).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    // ReSharper disable once AssignNullToNotNullAttribute
                    // ReSharper disable once PossibleNullReferenceException
                    tcs.TrySetException(t.Exception.InnerException);
                else
                    tcs.TrySetResult(t.Result);
            });

            return tcs.Task;
        }

        public Task<INikoResponse<IReadOnlyList<IAction>>> GetActions()
        {
            var tcs = new TaskCompletionSource<INikoResponse<IReadOnlyList<IAction>>>();
            SendCommand<IReadOnlyList<IAction>>(new GetActionsCommand(), new ActionsConverter()).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    // ReSharper disable once AssignNullToNotNullAttribute
                    // ReSharper disable once PossibleNullReferenceException
                    tcs.TrySetException(t.Exception.InnerException);
                else
                    tcs.TrySetResult(t.Result);
            });

            return tcs.Task;
        }

        public Task<IBaseResponse> ExecuteCommand(int id, int value)
        {
            var tcs = new TaskCompletionSource<IBaseResponse>();
            SendCommand<ErrorImp>(new ExecuteCommand(id,value), new BaseResponseConverter()).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    // ReSharper disable once AssignNullToNotNullAttribute
                    // ReSharper disable once PossibleNullReferenceException
                    tcs.TrySetException(t.Exception.InnerException);
                else
                {
                    var br = new BaseResponse(t.Result.Data.Error) { Command = t.Result.Command };
                    tcs.TrySetResult(br);
                }
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

            observer = _observableResponses.Subscribe(new ActionObserver<JObject>(jo =>
            {
                //Check if it is response from same command
                if (jo.ContainsKey("cmd") && jo["cmd"].Value<string>().Equals(command.CommandName, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        tcs.TrySetResult(Deserialize<T>(jo.ToString(), converters));
                    }
                    catch (Exception e)
                    {
                        tcs.TrySetException(e);
                    }
                    finally
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        // ReSharper disable once AccessToModifiedClosure
                        observer.Dispose();
                    }
                }
            }));

            try
            {
                _tcpClient.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }

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

            _tcpClient.Start();

            _readFromSocket = true;

#pragma warning disable 4014
            ReadData();
#pragma warning restore 4014
        }

        private async Task ReadData()
        {
            var buffer = new byte[10000];
            var index = 0;

            while (_readFromSocket)
            {
                var br = await _tcpClient.ReadAsync(buffer, index, buffer.Length - index);

                index += br;

                if (buffer[index - 2] == '\r' || buffer[index - 1] == '\n')
                {
                    var datastring = Encoding.ASCII.GetString(buffer, 0, index);

#if DEBUG
                    Debug.WriteLine("Message received : " + datastring);
#endif

                    foreach (var s in datastring.Split(new[] {"\r\n"},StringSplitOptions.RemoveEmptyEntries))
                    {
                        var jo = JObject.Parse(s);
                        if (jo.ContainsKey("event"))
                            SendEvent(s);
                        else
                            _observableResponses.Add(jo);
                    }
                    index = 0;
                }  
            }
        }

        public void StopClient()
        {
            if (IsConnected)
            {
                _readFromSocket = false;
                _tcpClient.Stop();
            }
        }

        private void SendEvent(string eventdata)
        {
            var theevent = JsonConvert.DeserializeObject<EventImp>(eventdata, new EventConverter());
            OnValueChanged?.Invoke(this, theevent);
        }

   
    }
}
