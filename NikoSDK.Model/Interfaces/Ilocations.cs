using System.Collections.Generic;

namespace NikoSDK.Interfaces.Data
{
    public interface ILocations : IBaseResponse
    {
        IReadOnlyList<ILocation> Locations { get; }
    }

}
