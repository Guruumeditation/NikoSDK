using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Action = Net.ArcanaStudio.NikoSDK.Models.Action;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    internal class ActionTypeConverter : JsonConverter<Action>
    {
        public override void WriteJson(JsonWriter writer, Action value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override Action ReadJson(JsonReader reader, Type objectType, Action existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var actionraw = JObject.Load(reader);
            return new Action(actionraw["id"].Value<int>(), actionraw["name"].Value<string>(), actionraw["type"].Value<int>(), actionraw["location"].Value<int>(), actionraw["value1"].Value<int>());

        }
    }
}
