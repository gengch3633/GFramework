using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class SDKConst
    {
        public static SdkConfig sdkConfig;
        static SDKConst()
        {
            sdkConfig = GameUtils.GetConfigInfos<SdkConfig>()[0];
        }

        public static string supportEmail = "daily_joy@outlook.com";
        public static string privacyPolicyPage = "https://www.dailyjoy.co.in/privacy_policy.html";

        public const string SDK_APPSFLYER_KEY = "vt7s3k2W3cAe9he5vbSiwm";
        public const string SDK_APPSFLYER_LINKID = "";
        public const string SDK_APPSFLYER_IOS_APPID = "";

        //重要！重要！重要！
        //工具栏Assets->Google Mobile Ads->Settings,填写APP ID
#if UNITY_IPHONE
    // Admob Mediation
    public const string SDK_ADMOB_AD_UNIT_ID_REWARDED = "";
    public const string SDK_ADMOB_AD_UNIT_ID_INTERSTITIAL = "";
    public const string SDK_ADMOB_AD_UNIT_ID_BANNER = "";
#else
        // Admob Mediation
        public const string SDK_ADMOB_AD_UNIT_ID_REWARDED = "ca-app-pub-2862865501975840/2672704330";
        public const string SDK_ADMOB_AD_UNIT_ID_INTERSTITIAL = "ca-app-pub-2862865501975840/3227276093";
        public const string SDK_ADMOB_AD_UNIT_ID_BANNER = "ca-app-pub-2862865501975840/1151777099";
#endif
    }
}