using Net.ArcanaStudio.NikoSDK.Models;
using Newtonsoft.Json.Linq;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    internal class SystemInfoConverter : BaseNikoResponseConverter<SystemInfo>
    {
        public override SystemInfo DeserializeData(JToken data)
        {
            var si = new SystemInfo(data["swversion"].ToString(), data["api"].ToString(), data["time"]?.ToString().ParseNikoDateTimeString(), data["language"].ToString(), data["currency"].ToString(), data["units"].ToString(), data["DST"].ToString(), data["TZ"].ToString(), data["lastenergyerase"]?.ToString().ParseNikoDateTimeString(), data["lastconfig"]?.ToString().ParseNikoDateTimeString());
            return si;
        }
    }
}
