using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
{
    [DataContract]
    public class Location : BaseResponse, ILocation
    {
        [DataMember]
        public int Id { get; }
        [DataMember]
        public string Name { get; }

        public Location(int id, string name, int error) : base(error)
        {
            Id = id;
            Name = name;
        }
    }
}
