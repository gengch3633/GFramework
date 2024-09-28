using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class SDKConst
    {
        private static SdkConfig sdkConfigProduction;

        public static SdkConfig SdkConfigProduction { get => sdkConfigProduction; }
        static SDKConst()
        {
            sdkConfigProduction = GameUtils.GetConfigInfos<SdkConfig>()[0];
        }
    }
}