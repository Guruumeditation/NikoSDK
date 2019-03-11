using System;
using System.Collections.Generic;
using Net.ArcanaStudio.NikoSDK.Models;
using Newtonsoft.Json;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    public class SystemInfoConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dict = new Dictionary<string, string>();
            serializer.Populate(reader, dict);

            if (dict.ContainsKey("error"))
            {
                 return new SystemInfo(int.Parse(dict["error"]));
            }
            
            return new SystemInfo(dict["swversion"],dict["api"],dict["time"]?.ParseNikoDateTimeString(),dict["language"],dict["currency"],dict["units"],dict["DST"],dict["TZ"],dict["lastenergyerase"]?.ParseNikoDateTimeString(),dict["lastconfig"]?.ParseNikoDateTimeString());
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SystemInfo);
        }
    }
}
