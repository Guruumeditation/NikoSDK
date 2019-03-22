using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
{
    [DataContract]
    public class EventItem : IEventItem
    {
        [DataMember]
        public int Id { get; }
        [DataMember]
        public int Value { get; }

        public EventItem(int id, int value)
        {
            Id = id;
            Value = value;
        }
    }
}
