using System.Collections.Generic;
using System.Linq;
using Net.ArcanaStudio.NikoSDK.Interfaces;
using Net.ArcanaStudio.NikoSDK.Models;
using Newtonsoft.Json.Linq;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    internal class LocationsConverter : BaseNikoResponseConverter<IReadOnlyList<ILocation>>
    {
        public override IReadOnlyList<ILocation> DeserializeData(JToken data)
        {
            var actions = data.ToObject<Location[]>();

            return actions.ToList().AsReadOnly();
        }
    }
}
