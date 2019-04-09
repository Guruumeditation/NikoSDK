using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK
{
    internal class NikoTcpClient : ITcpClient
    {
        private readonly int _port = 8000;
        private readonly string _host;
        private TcpClient _client;
        private NetworkStream _stream;

        public IPAddress IpAddress => ((IPEndPoint)_client?.Client.RemoteEndPoint)?.Address ?? IPAddress.None;

        public NikoTcpClient(string host)
        {
            _host = host;
        }

        public bool IsConnected => _client?.Connected ?? false;

        public void Start()
        {
            if (!IsConnected)
            {
                _client = new TcpClient(_host, _port);
                _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                _stream = _client.GetStream();
            }
        }

        public void Stop()
        {
            if (IsConnected)
                _client.Close();
        }

        public Task WriteAsync(byte[] buffer, int offset, int length)
        {
            return _stream.WriteAsync(buffer, offset, length);
        }

        public Task<int> ReadAsync(byte[] buffer, int offset, int length)
        {
            return _stream.ReadAsync(buffer, offset, length);
        }
    }
}
