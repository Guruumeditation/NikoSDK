using System.Collections.Generic;

namespace Net.ArcanaStudio.NikoSDK.Interfaces
{
    public interface ILocations : IBaseResponse
    {
        IReadOnlyList<ILocation> Locations { get; }
    }

}
