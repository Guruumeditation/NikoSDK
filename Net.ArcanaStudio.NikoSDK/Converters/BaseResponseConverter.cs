using Net.ArcanaStudio.NikoSDK.Model;
using Newtonsoft.Json.Linq;

namespace Net.ArcanaStudio.NikoSDK.Converters
{
    internal class BaseResponseConverter : BaseNikoResponseConverter<ErrorImp>
    {
        public override ErrorImp DeserializeData(JToken data)
        {
            var errorcode = data["error"].Value<int>();

            return new ErrorImp{Error = errorcode };
        }
    }
}
