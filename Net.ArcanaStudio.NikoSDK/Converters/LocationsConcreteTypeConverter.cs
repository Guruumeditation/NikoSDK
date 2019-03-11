using System;
using System.Collections.Generic;
using Net.ArcanaStudio.NikoSDK.Interfaces;
using Net.ArcanaStudio.NikoSDK.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    internal class LocationsConcreteTypeConverter : ConcreteTypeConverter<ILocations, LocationsImp>
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var errorpath = nameof(IBaseResponse.Error).ToLower();

            LocationsImp locations = null;

            try
            {
                var jarray = JArray.Load(reader);
                locations = new LocationsImp(jarray.ToObject<List<Location>>().AsReadOnly());
            }
            catch (Exception)
            {
                var jsobject = JObject.Load(reader);
                locations = new LocationsImp(jsobject[errorpath].Value<int>());
            }

            return locations;
        }
    }
}
