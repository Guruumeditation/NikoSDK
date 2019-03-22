using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
{
    [DataContract]
    public class BaseResponse: IBaseResponse
    {
        [DataMember]
        public int Error { get; }

        [IgnoreDataMember] public bool IsError => Error != 0;

        public BaseResponse(int error)
        {
            Error = error;
        }
    }
}
