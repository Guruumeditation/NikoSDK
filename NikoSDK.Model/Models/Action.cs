using System.Runtime.Serialization;
using NikoSDK.Interfaces.Data;

namespace NikoSDK.Model.Data
{
    [DataContract]
    public class Action :IAction
    {
        [DataMember(Name = "id")]
        public int Id { get; }
        [DataMember(Name = "name")]
        public string Name { get; }
        [DataMember(Name = "type")]
        public int Type { get;}
        [DataMember(Name = "location")]
        public int Location { get;}
        [DataMember(Name = "value1")]
        public int Value { get; }

        public Action(int id, string name, int type, int location, int value)
        {
            Id = id;
            Name = name;
            Type = type;
            Location = location;
            Value = value;
        }
    }
}
