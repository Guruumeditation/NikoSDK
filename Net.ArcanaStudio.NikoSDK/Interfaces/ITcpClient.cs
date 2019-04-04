using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Net.ArcanaStudio.NikoSDK.Interfaces
{
    public interface ITcpClient
    {
        bool IsConnected { get; }
        IPAddress IpAddress { get; }
        void Start();
        void Stop();
        Task WriteAsync(byte[] buffer, int offset, int length);
        Task<int> ReadAsync(byte[] buffer, int offset, int length);
    }
}
