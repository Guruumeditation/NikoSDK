using System.Collections.Generic;
using System.Linq;
using Net.ArcanaStudio.NikoSDK.Interfaces;
using Newtonsoft.Json.Linq;
using Action = Net.ArcanaStudio.NikoSDK.Models.Action;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    internal class ActionsConverter : BaseNikoResponseConverter<IReadOnlyList<IAction>>
    {
        public override IReadOnlyList<IAction> DeserializeData(JToken data)
        {
            var actions = data.ToObject<Action[]>();

            return actions.ToList().AsReadOnly();
        }
    }
}
