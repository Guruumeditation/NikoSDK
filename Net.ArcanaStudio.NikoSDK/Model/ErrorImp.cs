using System.Runtime.Serialization;

namespace Net.ArcanaStudio.NikoSDK.Model
{
    [DataContract]
    internal class ErrorImp
    {
        [DataMember]
        public int Error { get; set; }
    }
}
