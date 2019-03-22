using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
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
