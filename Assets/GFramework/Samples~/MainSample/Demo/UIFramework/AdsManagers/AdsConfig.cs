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
        public bool useTestAds;
        [JsonConverter(typeof(StringEnumConverter))]
        public BuildTarget buildTarget;
        [JsonConverter(typeof(StringEnumConverter))]
        public EMediationType mediationType;
        public string appId;
        public string rewardAdId;
        public string interstitialAdId;
        public string bannerAdId;
        public string splashAdId;

        public void SetAdsIds(AdsConfig otherAdsConfig)
        {
            this.rewardAdId = otherAdsConfig.rewardAdId;
            this.interstitialAdId = otherAdsConfig.interstitialAdId;
            this.bannerAdId = otherAdsConfig.bannerAdId;
            this.splashAdId = otherAdsConfig.splashAdId;
        }
    }

    public enum EMediationType
    {
        Admob,
        Max,
        TopOn
    }
}

