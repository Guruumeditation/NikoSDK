using System;
using Net.ArcanaStudio.NikoSDK.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    public abstract class BaseNikoResponseConverter<T> : JsonConverter<NikoResponse<T>>
    {
        public override void WriteJson(JsonWriter writer, NikoResponse<T> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override NikoResponse<T> ReadJson(JsonReader reader, Type objectType, NikoResponse<T> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var jobject = JObject.Load(reader);

            if (jobject.ContainsKey("error"))
            {
                return new NikoResponse<T>(jobject["error"].Value<int>());
            }

            var jtoken = jobject["data"];

            var data = DeserializeData(jtoken);

            return new NikoResponse<T>(jobject["cmd"].Value<string>(),data);
        }

        public abstract T DeserializeData(JToken data);
    }
}
