using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GameFramework
{
    public class SDKConst
    {
        public static SdkConfig sdkConfig;

        static SDKConst()
        {
            var sdkConfigs = GameUtils.GetConfigInfos<SdkConfig>();
            var buildTarget = GameUtils.IsIosPlatform() ? BuildTarget.IOS : BuildTarget.Android;
            sdkConfig = sdkConfigs.Find(item=>item.buildTarget == buildTarget);
            Debug.Log($"[SDKConst] [Release] sdkConfig: {JsonConvert.SerializeObject(sdkConfig)}");
        }
    }
}