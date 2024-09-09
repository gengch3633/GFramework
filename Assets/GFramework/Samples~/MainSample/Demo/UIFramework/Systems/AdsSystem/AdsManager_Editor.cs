using Framework;
using System;
using UnityEngine;

namespace GameFramework
{
    public class AdsManager_Editor : IAdsManager, ITypeLog
    {
        private int rewardCheckFailedCount = 0;
        private bool isIntAdReturnSuccess = true;
        private bool isRewardAdReturnSuccess = true;
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
            var ret = isIntAdReturnSuccess;
            isIntAdReturnSuccess = !isIntAdReturnSuccess;
            if (IsTypeLogEnabled()) Debug.LogError("==> [EditorAdsManager] [IsInterstitialAdAvailable]: " + ret);
            return isIntAdReturnSuccess;
        }

        public void ShowInterstitialAd(string place, Action<bool> showCompletedCallback)
        {
            if(IsTypeLogEnabled()) Debug.LogError("==> [EditorAdsManager] [ShowInterstitialAd]: " + place);
            showCompletedCallback?.Invoke(true);
        }


        public bool IsRewardAdReady()
        {
            var ret = isRewardAdReturnSuccess;

            if (isRewardAdReturnSuccess)
                isRewardAdReturnSuccess = !isRewardAdReturnSuccess;
            else
            {
                rewardCheckFailedCount++;
                if (rewardCheckFailedCount >= 6)
                {
                    rewardCheckFailedCount = 0;
                    isRewardAdReturnSuccess = !isRewardAdReturnSuccess;
                }
            }

            if (IsTypeLogEnabled()) Debug.LogError("==> [EditorAdsManager] [IsRewardAdAvailable]: " + ret);
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

