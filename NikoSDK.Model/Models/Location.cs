using System.Runtime.Serialization;
using NikoSDK.Interfaces.Data;

namespace NikoSDK.Model.Data
{
    [DataContract]
    public class Location : BaseResponse, ILocation
    {
        [DataMember(Name = "id")]
        public int Id { get; }
        [DataMember(Name = "name")]
        public string Name { get; }

        public Location(int id, string name, int error) : base(error)
        {
            Id = id;
            Name = name;
        }
    }
}
