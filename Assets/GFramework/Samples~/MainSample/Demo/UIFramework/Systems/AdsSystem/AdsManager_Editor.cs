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
            return GameUtils.IsTypeLogEnabled(this);
        }
        public void ShowBanner()
        {
            GameUtils.Log(this);
        }
        public void HideBanner()
        {
            GameUtils.Log(this);
        }

        public bool IsInterstitialAdReady()
        {
            var ret = isIntAdReturnSuccess;
            isIntAdReturnSuccess = !isIntAdReturnSuccess;
            GameUtils.Log(this, $"ret: {ret}");
            return isIntAdReturnSuccess;
        }

        public void ShowInterstitialAd(string place, Action<bool> showCompletedCallback)
        {
            GameUtils.Log(this, $"place: {place}");
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

            GameUtils.Log(this, $"ret: {ret}");
            return ret;
        }
        
        public void ShowRewardAd(string place, Action<bool> showCompletedCallback)
        {
            GameUtils.Log(this, $"place: {place}");
            showCompletedCallback?.Invoke(true);
        }

        public bool IsApplicationFocusFromAds()
        {
            return false;
        }
    }
}

