using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
{
    [DataContract]
    public class BaseResponse: IBaseResponse
    {
        [DataMember(Name = "cmd")]
        public string Command { get; internal set; }

        [DataMember]
        public int Error { get; }

        [IgnoreDataMember] public bool IsError => Error != 0;

        public BaseResponse(int error = 0)
        {
            Error = error;
        }
    }
}
