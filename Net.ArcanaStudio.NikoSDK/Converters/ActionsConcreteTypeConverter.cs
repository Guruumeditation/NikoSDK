using System;
using System.Collections.Generic;
using Net.ArcanaStudio.NikoSDK.Interfaces;
using Net.ArcanaStudio.NikoSDK.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Action = Net.ArcanaStudio.NikoSDK.Models.Action;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    internal class ActionsConcreteTypeConverter : ConcreteTypeConverter<IActions, ActionsImp>
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var errorpath = nameof(IBaseResponse.Error).ToLower();

            ActionsImp actions = null;

            try
            {
                var jarray = JArray.Load(reader);
                var actionslist = new List<Action>();
                foreach (var actionraw in jarray)
                {
                    actionslist.Add(JsonConvert.DeserializeObject<Action>(actionraw.ToString(), new ActionTypeConverter()));
                }
                actions = new ActionsImp(actionslist);
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
