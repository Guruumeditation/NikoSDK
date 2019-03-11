using System;
using Newtonsoft.Json;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    public class ConcreteTypeConverter<TInterface,TConcrete> : JsonConverter
    {
        private readonly string _interfaceName = typeof(TInterface).FullName;
        public override bool CanConvert(Type objectType)
        {
            //assume we can convert to anything for now
            return objectType.FullName.Equals((_interfaceName));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //explicitly specify the concrete type we want to create
            return serializer.Deserialize<TConcrete>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //use the default serialization - it works fine
            serializer.Serialize(writer, value);
        }
    }

}
