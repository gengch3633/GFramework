using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace GameFramework
{
    public class AdsConfig
    {
        public int id;
        [JsonConverter(typeof(StringEnumConverter))]
        public EAdsType adsType;
        public string rewardAdId;
        public string interstitialAdId;
        public string bannerAdId;
        public string splashAdId;
    }

    public enum EAdsType
    {
        AdmobAndroid,
        AdmobIOS,
        AdmobAndroidTest,
        AdmobIOSTest
    }
}

