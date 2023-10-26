using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class SDKConst
    {
        private static SdkConfig sdkConfigProduction;
        private static SdkConfig sdkConfigDevelopment;

        public static SdkConfig SdkConfigProduction { get => sdkConfigProduction; }
        public static SdkConfig SdkConfigDevelopment { get => sdkConfigDevelopment; }
        static SDKConst()
        {
            var sdkConfigs = GameUtils.GetConfigInfos<SdkConfig>();
            sdkConfigProduction = sdkConfigs.Find(item=> !item.isTest);
            sdkConfigDevelopment = sdkConfigs.Find(item => item.isTest);
        }

        public const string SDK_APPSFLYER_LINKID = "";
        public const string SDK_APPSFLYER_IOS_APPID = "";
    }
}