using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    internal class ConcreteListTypeConverter<TInterface, TImplementation> : JsonConverter where TImplementation : TInterface
    {
        private readonly string _listTypeName = typeof(IList<TInterface>).FullName;
        public override bool CanConvert(Type objectType)
        {
            return objectType.FullName.Equals(_listTypeName);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var res = serializer.Deserialize<List<TImplementation>>(reader);
            return res.ConvertAll(x => (TInterface)x);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
