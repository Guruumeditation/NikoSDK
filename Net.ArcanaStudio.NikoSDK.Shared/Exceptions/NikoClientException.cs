using System;

namespace Net.ArcanaStudio.NikoSDK.Shared.Exceptions
{
    public class NikoClientException : ApplicationException
    {
        public NikoClientException(string message) : base(message)
        {
        }
    }
}
