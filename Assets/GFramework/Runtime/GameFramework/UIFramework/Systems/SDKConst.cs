
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SDKConst
{
    public static string supportEmail = "PJGameStudio@outlook.com";
    public static string privacyPolicyPage = "https://sites.google.com/view/pjgame/privacypolicy";
    public static string termsOfServicePage = "http://sites.google.com/view/pjoygames-termsofservice";

    public const string SDK_APPSFLYER_KEY = "ax6bY5957HVALFEZWJZcL8";
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
    public const string SDK_ADMOB_AD_UNIT_ID_REWARDED = "ca-app-pub-4624434356190386/2210123185";
    public const string SDK_ADMOB_AD_UNIT_ID_INTERSTITIAL = "ca-app-pub-4624434356190386/7348530108";
    public const string SDK_ADMOB_AD_UNIT_ID_BANNER = "ca-app-pub-4624434356190386/7406609543";
#endif
}
