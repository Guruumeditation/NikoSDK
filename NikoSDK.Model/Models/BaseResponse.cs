using System.Runtime.Serialization;
using NikoSDK.Interfaces.Data;

namespace NikoSDK.Model.Data
{
    [DataContract]
    public class BaseResponse: IBaseResponse
    {
        [DataMember(Name = "error")]
        public int Error { get; }

        [IgnoreDataMember] public bool IsError => Error != 0;

        public BaseResponse(int error)
        {
            Error = error;
        }
    }
}
