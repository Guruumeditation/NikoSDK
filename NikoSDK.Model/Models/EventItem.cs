using System.Runtime.Serialization;
using NikoSDK.Interfaces.Data;

namespace NikoSDK.Model.Data
{
    [DataContract]
    public class EventItem : IEventItem
    {
        [DataMember(Name = "id")]
        public int Id { get; }
        [DataMember(Name = "value1")]
        public int Value { get; }

        public EventItem(int id, int value)
        {
            Id = id;
            Value = value;
        }
    }
}
