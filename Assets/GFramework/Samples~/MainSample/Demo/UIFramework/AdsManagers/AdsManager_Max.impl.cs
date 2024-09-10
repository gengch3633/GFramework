
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public partial class AdsManager_Max //: ITypeLog
    {
        //public bool IsTypeLogEnabled()
        //{
        //    var debugSystem = GameApp.Interface.GetSystem<IDebugSystem>();
        //    var ret = debugSystem.IsTypeLogEnabled(typeof(AdsManager_Admob).FullName);
        //    return ret;
        //}
        //public bool InitSuccessFlag { get; private set; } = false;

        //private int interstitialAdRetryAttempt;
        //private int rewardedAdRetryAttempt;

        //private Action<bool> interstitialAdCallBack;
        //private Action<bool> rewardedAdCallBack;

        //private int playRewardADCount = 0;
        //private bool rewardADReceivedReward = false;

        //public void Init()
        //{
        //    if (!InitSuccessFlag)
        //    {
        //        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        //        {
        //        // AppLovin SDK is initialized, start loading ads
        //        Debug.Log("Applovin Max Inititial success.");
        //            InitializeInterstitialAds();
        //            InitializeRewardedAds();
        //        //InitializeBannerAds();
        //        InitializeMRecAds();
        //            InitSuccessFlag = true;
        //        };
        //        MaxSdk.SetSdkKey(SDKConst.SDK_MAX_KEY);
        //        MaxSdk.InitializeSdk();
        //    }

        //}

        //#region InterstitialAds

        //public void InitializeInterstitialAds()
        //{
        //    // Attach callback
        //    //MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        //    //MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        //    //MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        //    //MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;

        //    MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        //    MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
        //    MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
        //    MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        //    MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        //    // Load the first interstitial
        //    LoadInterstitial();
        //}

        //private void LoadInterstitial()
        //{
        //    MaxSdk.LoadInterstitial(SDKConst.SDK_MAX_AD_UNIT_ID_INTERSTITIAL);
        //}

        ////private void OnInterstitialLoadedEvent(string adUnitId)
        ////{
        ////    // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'

        ////    // Reset retry attempt
        ////    interstitialAdRetryAttempt = 0;
        ////}

        //private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo info)
        //{
        //    // Reset retry attempt
        //    interstitialAdRetryAttempt = 0;
        //}

        //private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        //{
        //    // Interstitial ad failed to load 
        //    // We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds)

        //    interstitialAdRetryAttempt++;
        //    double retryDelay = Math.Pow(2, Math.Min(6, interstitialAdRetryAttempt));

        //    //Invoke("LoadInterstitial", (float)retryDelay);
        //    WaitForSecondsDo((float)retryDelay, LoadInterstitial).Forget();
        //}

        //private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo info)
        //{
        //    isShowingAd = false;
        //    // Interstitial ad failed to display. We recommend loading the next ad
        //    LoadInterstitial();
        //}

        //private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo info)
        //{
        //    interstitialAdCallBack?.Invoke(true);

        //    // Interstitial ad is hidden. Pre-load the next ad
        //    LoadInterstitial();

        //    //SDKManager.AddShowADInterCount();
        //}
        //#endregion

        //#region Rewarded Ads



        //public void InitializeRewardedAds()
        //{
        //    // Attach callback
        //    //MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        //    //MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        //    //MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        //    //MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        //    //MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        //    //MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        //    //MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        //    MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        //    MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        //    MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        //    MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        //    MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        //    MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        //    MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        //    MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        //    // Load the first RewardedAd
        //    LoadRewardedAd();
        //}
        

       

        //private void LoadRewardedAd()
        //{
        //    MaxSdk.LoadRewardedAd(SDKConst.SDK_MAX_AD_UNIT_ID_REWARDED);
        //}

        //private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo info)
        //{
        //    // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'

        //    // Reset retry attempt
        //    rewardedAdRetryAttempt = 0;
        //}

        //private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        //{
        //    // Rewarded ad failed to load 
        //    // We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds)

        //    rewardedAdRetryAttempt++;
        //    double retryDelay = Math.Pow(2, Math.Min(6, rewardedAdRetryAttempt));

        //    //Invoke("LoadRewardedAd", (float)retryDelay);
        //    WaitForSecondsDo((float)retryDelay, LoadRewardedAd).Forget();
        //}

        //private async UniTask WaitForSecondsDo(float delayTime, Action action)
        //{
        //    await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
        //    action.Invoke();
        //}

        //private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo info)
        //{
        //    isShowingAd = false;
        //    // Rewarded ad failed to display. We recommend loading the next ad
        //    LoadRewardedAd();
        //}

        //private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo info) { }

        //private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo info) { }

        //private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo inf)
        //{
        //    //Debug.Log($"OnRewardedAdDismissedEvent rewardedAdCallBack playRewardADCount:{playRewardADCount},rewardADReceivedReward:{rewardADReceivedReward}");
        //    //rewardedAdCallBack?.Invoke(rewardADReceivedReward);
        //    //OnRewardedAdReceivedRewardEvent 会有不被调用的bug，这里关闭后直接发奖励
        //    rewardedAdCallBack?.Invoke(true);
        //    // Rewarded ad is hidden. Pre-load the next ad
        //    LoadRewardedAd();

        //    playRewardADCount++;

        //    //非自然流量用户才会有3拖1
        //    //if (playRewardADCount >= 3 && !SDKManager.GetAppsFlyerOrganicInstallStatus())
        //    //{
        //    //    playRewardADCount = 0;
        //    //    SDKManager.ShowInterstitialAds("Extra", null);
        //    //}

        //    //SDKManager.AddShowADRewardCount();
        //}

        //private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo info)
        //{
        //    // Rewarded ad was displayed and user should receive the reward
        //    rewardADReceivedReward = true;
        //    Debug.Log("OnRewardedAdReceivedRewardEvent ...");

        //}
        //#endregion

        //#region Banners 
        //public void InitializeBannerAds()
        //{
        //    // Banners are automatically sized to 320x50 on phones and 728x90 on tablets
        //    // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments
        //    MaxSdk.CreateBanner(SDKConst.SDK_MAX_AD_UNIT_ID_BANNER, MaxSdkBase.BannerPosition.BottomCenter);

        //    // Set background or background color for banners to be fully functional
        //    //Color bannerColor = new Color(1f, 1f, 1f, 0f);
        //    Color32 bannerColor = new Color32(0, 104, 5, 1);

        //    MaxSdk.SetBannerBackgroundColor(SDKConst.SDK_MAX_AD_UNIT_ID_BANNER, bannerColor);
        //}

        //public void ShowBanner()
        //{
        //    MaxSdk.ShowBanner(SDKConst.SDK_MAX_AD_UNIT_ID_BANNER);
        //    Debug.Log($"MAXSDKManager ShowBanner 。。。。ID:{SDKConst.SDK_MAX_AD_UNIT_ID_BANNER}");
        //}

        //public void HideBanner()
        //{
        //    MaxSdk.HideBanner(SDKConst.SDK_MAX_AD_UNIT_ID_BANNER);
        //}
        //#endregion

        //#region MRec

        //public void InitializeMRecAds()
        //{
        //    // MRECs are sized to 300x250 on phones and tablets
        //    MaxSdk.CreateMRec(SDKConst.SDK_MAX_AD_UNIT_ID_MREC, MaxSdkBase.AdViewPosition.Centered);
        //    //Debug.Log($"InitializeMRecAds:{SDKConst.SDK_MAX_AD_UNIT_ID_MREC}");
        //}

        //public void UpdateMRecPosition(MaxSdkBase.AdViewPosition mrecPosition)
        //{
        //    //Debug.Log($"UpdateMRecPosition:{mrecPosition}");
        //    MaxSdk.UpdateMRecPosition(SDKConst.SDK_MAX_AD_UNIT_ID_MREC, mrecPosition);
        //}

        //public void ShowMRec()
        //{
        //    MaxSdk.ShowMRec(SDKConst.SDK_MAX_AD_UNIT_ID_MREC);
        //    //Debug.Log($"ShowMRec:{SDKConst.SDK_MAX_AD_UNIT_ID_MREC}");
        //}

        //public void HideMRec()
        //{
        //    MaxSdk.HideMRec(SDKConst.SDK_MAX_AD_UNIT_ID_MREC);
        //    //Debug.Log($"HideMRec:{SDKConst.SDK_MAX_AD_UNIT_ID_MREC}");
        //}

        //public Rect GetMRecLayout()
        //{
        //    return MaxSdk.GetMRecLayout(SDKConst.SDK_MAX_AD_UNIT_ID_MREC);
        //}

        //public float GetScreenDensity()
        //{
        //    return MaxSdkUtils.GetScreenDensity();
        //}
        //#endregion




        //private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        //{

        //    //Note: The value of Revenue may be -1 in the case of an error.
        //    double revenue = adInfo.Revenue;

        //    // Miscellaneous data
        //    string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        //    string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        //    string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        //    string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
        //    string networkPlacement = adInfo.NetworkPlacement; // The placement ID from the network that showed the ad
        //    Debug.Log($"******:revenue={revenue}, countryCode={countryCode}, networkName={networkName}," +
        //        $"adUnitIdentifier={adUnitIdentifier}, placement={placement}, networkPlacement={networkPlacement}");
        //}


    }
}


