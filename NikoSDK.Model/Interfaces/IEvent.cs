using System.Collections.Generic;

namespace NikoSDK.Interfaces.Data
{
    public interface IEvent
    {
        string Event { get; }
        IReadOnlyList<IEventItem> Data { get; }
    }
}
