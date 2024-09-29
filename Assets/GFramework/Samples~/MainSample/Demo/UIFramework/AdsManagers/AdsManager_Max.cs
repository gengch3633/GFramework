
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace GameFramework
{
    public partial class AdsManager_Max //: IAdsManager
    {
        //private bool isShowingAd = false;

        //public AdsManager_Max()
        //{
        //    Application.focusChanged += OnApplicationFocus;
        //}

        //private async void OnApplicationFocus(bool focus)
        //{
        //    if (focus)
        //    {
        //        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        //        isShowingAd = false;
        //    }
        //}

        //public bool IsApplicationFocusFromAds()
        //{
        //    return isShowingAd;
        //}

        //public bool IsInterstitialAdReady()
        //{
        //    return MaxSdk.IsInterstitialReady(SDKConst.SDK_MAX_AD_UNIT_ID_INTERSTITIAL);
        //}

        //public bool IsRewardAdReady()
        //{
        //    return MaxSdk.IsRewardedAdReady(SDKConst.SDK_MAX_AD_UNIT_ID_REWARDED);
        //}


        //public void ShowInterstitialAd(string place, Action<bool> showCompletedCallback = null)
        //{
        //    isShowingAd = true;
        //    interstitialAdCallBack = showCompletedCallback;
        //    MaxSdk.ShowInterstitial(SDKConst.SDK_MAX_AD_UNIT_ID_INTERSTITIAL);
        //    EventUtils.LogAdRealMaxInterstitialEvent();
        //}

        //public void ShowRewardAd(string place, Action<bool> showCompletedCallback)
        //{
        //    isShowingAd = true;
        //    rewardedAdCallBack = showCompletedCallback;
        //    rewardADReceivedReward = false;
        //    MaxSdk.ShowRewardedAd(SDKConst.SDK_MAX_AD_UNIT_ID_REWARDED);
        //    EventUtils.LogAdRealMaxRewardEvent();
        //}
    }
}


