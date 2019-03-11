using System;
using System.Collections.Generic;
using Net.ArcanaStudio.NikoSDK.Interfaces;
using Net.ArcanaStudio.NikoSDK.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Action = Net.ArcanaStudio.NikoSDK.Models.Action;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    public class ActionConcreteTypeConverter : ConcreteTypeConverter<IActions, ActionsImp>
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var errorpath = nameof(IBaseResponse.Error).ToLower();

            ActionsImp actions = null;

            try
            {
                var jarray = JArray.Load(reader);
                actions = new ActionsImp(jarray.ToObject<List<Action>>().AsReadOnly());
            }
            catch (Exception)
            {
                var jsobject = JObject.Load(reader);
                actions = new ActionsImp(jsobject[errorpath].Value<int>());
            }

            return actions;
        }
    }
}
