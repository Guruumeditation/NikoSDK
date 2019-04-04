using System;
using Net.ArcanaStudio.NikoSDK.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    public class EventConverter : JsonConverter<EventImp>
    {
        #region Overrides of JsonConverter<EventImp>

        public override void WriteJson(JsonWriter writer, EventImp value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override EventImp ReadJson(JsonReader reader, Type objectType, EventImp existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var jobject = JObject.Load(reader);

            var jtoken = jobject["data"];

            var data = jtoken.ToObject<EventItem[]>();

            return new EventImp(jobject["event"].Value<string>(), data);
        }

        #endregion
    }
}
