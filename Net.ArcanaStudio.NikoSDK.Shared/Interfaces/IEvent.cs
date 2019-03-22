using System.Collections.Generic;

namespace Net.ArcanaStudio.NikoSDK.Interfaces
{
    public interface IEvent
    {
        string Event { get; }
        IReadOnlyList<IEventItem> Data { get; }
    }
}
