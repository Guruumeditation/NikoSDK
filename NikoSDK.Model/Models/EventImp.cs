using System.Collections.Generic;
using System.Runtime.Serialization;
using NikoSDK.Interfaces.Data;

namespace NikoSDK.Model.Data
{
    [DataContract]
    public class EventImp : IEvent
    {
        [DataMember(Name = "event")]
        public string Event { get; }
        [DataMember(Name = "data")]
        public IReadOnlyList<IEventItem> Data { get; }

        public EventImp(string @event, IReadOnlyList<IEventItem> data)
        {
            Event = @event;
            Data = data;
        }
    }
}
