using System.Collections.Generic;
using Net.ArcanaStudio.NikoSDK.Models;
using Newtonsoft.Json.Linq;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    internal class SystemInfoConverter : BaseNikoResponseConverter<SystemInfo>
    {
        public override SystemInfo DeserializeData(JToken data)
        {
            var si = new SystemInfo(data.ToObject<Dictionary<string,object>>());
            return si;
        }
    }
}
