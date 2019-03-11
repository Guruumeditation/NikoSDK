using System.Collections.Generic;
using System.Runtime.Serialization;
using NikoSDK.Interfaces.Data;

namespace NikoSDK.Model.Data
{
    [CollectionDataContract]
    public class LocationsImp: ILocations
    {
        [DataMember(Name = "error")]
        public int Error { get; }
        [IgnoreDataMember] public bool IsError => Error != 0;
        public IReadOnlyList<ILocation> Locations { get; }

        public LocationsImp(IReadOnlyList<ILocation> locations)
        {
            Locations = locations;
        }

        public LocationsImp(int error)
        {
            Error = error;
        }
    }
}
