using System.Runtime.Serialization;
using NikoSDK.Interfaces;

namespace NikoSDK.Model.Responses
{
    [DataContract]
    public class NikoResponse<T> : INikoResponse<T>
    {
        [DataMember(Name = "cmd")]
        public string Command { get; internal set; }
        [DataMember(Name = "data")]
        public T Data { get; internal set; }
    }
}
