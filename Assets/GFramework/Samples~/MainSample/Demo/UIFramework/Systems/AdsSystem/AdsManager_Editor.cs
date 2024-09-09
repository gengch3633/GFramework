using Framework;
using System;
using UnityEngine;

namespace GameFramework
{
    public class AdsManager_Editor : IAdsManager, ITypeLog
    {
        public bool IsTypeLogEnabled()
        {
            var debugSystem = GameApp.Interface.GetModel<IDebugModel>();
            var ret = debugSystem.IsTypeLogEnabled(this);
            return ret;

        }
        public void ShowBanner()
        {
            if(IsTypeLogEnabled()) Debug.LogError("==> [EditorAdsManager] [CloseBanner]");
        }
        public void HideBanner()
        {
            if(IsTypeLogEnabled()) Debug.LogError("==> [EditorAdsManager] [CloseBanner]");
        }

        public bool IsInterstitialAdReady()
        {
            var ret = true;
            if(IsTypeLogEnabled()) Debug.LogError("==> [EditorAdsManager] [IsInterstitialAdAvailable]: " + ret);
            return ret;
        }

        public void ShowInterstitialAd(string place, Action<bool> showCompletedCallback)
        {
            if(IsTypeLogEnabled()) Debug.LogError("==> [EditorAdsManager] [ShowInterstitialAd]: " + place);
            showCompletedCallback?.Invoke(true);
        }


        public bool IsRewardAdReady()
        {
            var ret = true;
            if(IsTypeLogEnabled()) Debug.LogError("==> [EditorAdsManager] [IsRewardAdAvailable]: " + ret);
            return ret;
        }
        
        public void ShowRewardAd(string place, Action<bool> showCompletedCallback)
        {
            if(IsTypeLogEnabled()) Debug.LogError("==> [EditorAdsManager] [ShowRewardAd]: " + place);
            showCompletedCallback?.Invoke(true);
        }

        public bool IsApplicationFocusFromAds()
        {
            return false;
        }
    }
}

