using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
{
    [DataContract]
    public class Action :IAction
    {
        [DataMember]
        public int Id { get; }
        [DataMember]
        public string Name { get; }
        [DataMember]
        public int Type { get;}
        [DataMember(Name = "Location")]
        public int LocationId { get;}
        [DataMember(Name = "value1")]
        public int Value { get; }

        public Action(int id, string name, int type, int location, int value)
        {
            Id = id;
            Name = name;
            Type = type;
            LocationId = location;
            Value = value;
        }
    }
}
