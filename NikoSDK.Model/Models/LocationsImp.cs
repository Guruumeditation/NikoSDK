using System.Collections.Generic;
using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
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
