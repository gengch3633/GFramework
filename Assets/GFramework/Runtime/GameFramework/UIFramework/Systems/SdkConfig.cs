using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameFramework
{
    public class SdkConfig
    {
        public int id;
        [JsonConverter(typeof(StringEnumConverter))]
        public BuildTarget buildTarget;
        public string supportEmail;
        public string privacyPage;

        public string facebookAppId;
        public string facebookToken;

        public string tenjinSdkKey;
        public string appsFlyerKey;
    }
}