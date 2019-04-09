using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
{
    [DataContract]
    public class NikoResponse<T> : BaseResponse, INikoResponse<T>
    {
        [DataMember(Name = "data")]
        public T Data { get; internal set; }

        public NikoResponse(int error) : base(error)
        {
        }

        public NikoResponse(string command,T data)
        {
            Command = command;
            Data = data;
        }
    }
}
