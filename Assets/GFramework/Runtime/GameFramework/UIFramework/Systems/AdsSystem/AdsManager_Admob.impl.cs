
using AudienceNetwork;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public partial class AdsManager_Admob: ITypeLog
    {
        public bool IsTypeLogEnabled()
        {
            var debugSystem = GameApp.Interface.GetSystem<IDebugSystem>();
            var ret = debugSystem.IsTypeLogEnabled(typeof(AdsManager_Admob).FullName);
            return ret;
        }
        public void Init()
        {
            AdSettings.SetDataProcessingOptions(new string[] { });
            if (IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] Init 1");

            MobileAds.SetiOSAppPauseOnBackground(true);
            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] Init 2");
            List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };
            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] Init 3");

            RequestConfiguration requestConfiguration =
                new RequestConfiguration.Builder()
                .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
                .SetTestDeviceIds(deviceIds).build();
            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] Init 4");

            MobileAds.SetRequestConfiguration(requestConfiguration);
            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] Init 5");
            MobileAds.Initialize(HandleInitCompleteAction);
            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] Init 6");
        }

        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {

            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] HandleInitCompleteAction 1");
            //gengch
            //#if UNITY_IOS
            //        AdSettings.SetAdvertiserTrackingEnabled(ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED);
            //#endif

            Dictionary<string, AdapterStatus> map = initstatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        break;
                    case AdapterState.Ready:
                        break;
                }

                if (IsTypeLogEnabled()) Debug.LogError($"==> [AdsManager_Admob] HandleInitCompleteAction 2, {className}: {status.InitializationState}");
            }

            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] HandleInitCompleteAction 3");
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                RequestAndLoadRewardedAd();
                RequestAndLoadInterstitialAd();
                RequestBannerAd();
            });
        }

        private void RequestAndLoadRewardedAd()
        {
            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] RequestAndLoadRewardedAd Start");
            rewardedAd?.Destroy();
            string adUnitId = SDKConst.SDK_ADMOB_AD_UNIT_ID_REWARDED;
            RewardedAd.Load(adUnitId, CreateAdRequest(), async (RewardedAd ad, LoadAdError loadError) =>
            {
                if (loadError != null || ad == null)
                {
                    if (IsTypeLogEnabled()) Debug.LogError($"==> [AdsManager_Admob] RewardAd Show Failed With Error: {loadError.GetMessage()}");
                    rewardedAdRetryAttempt++;
                    double retryDelay = Math.Pow(2, Math.Min(6, rewardedAdRetryAttempt));
                    await UniTask.Delay(TimeSpan.FromSeconds(retryDelay));
                    RequestAndLoadRewardedAd();
                    return;
                }

                if (IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] RequestAndLoadRewardedAd Loaded");
                rewardedAd = ad;
                rewardedAdRetryAttempt = 0;

                ad.OnAdFullScreenContentOpened += () => { isShowingAd = true; };
                ad.OnAdFullScreenContentClosed += () => RequestAndLoadRewardedAd();
                ad.OnAdImpressionRecorded += () => { };
                ad.OnAdClicked += () => { };
                ad.OnAdFullScreenContentFailed += (AdError error) => 
                { 
                    if (IsTypeLogEnabled()) Debug.LogError($"==> [AdsManager_Admob] RewardAd Show Failed With Error: {error.GetMessage()}");
                };
            });
        }

        private void RequestBannerAd()
        {
            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] RequestBannerAd Start");
            bannerView?.Destroy();
            string adUnitId = SDKConst.SDK_ADMOB_AD_UNIT_ID_BANNER;
            bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

            bannerView.OnBannerAdLoaded += () => { };
            bannerView.OnBannerAdLoadFailed += (LoadAdError error) => { };
            bannerView.OnAdFullScreenContentOpened += () => { };
            bannerView.OnAdFullScreenContentClosed += () => { };
            bannerView.OnAdPaid += (AdValue adValue) => { };
            bannerView.OnAdClicked += () => { isShowingAd = true; };

            bannerView.LoadAd(CreateAdRequest());
            HideBanner();
        }

        private void RequestAndLoadInterstitialAd()
        {
            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] RequestAndLoadInterstitialAd Start");
            interstitialAd?.Destroy();
            string adUnitId = SDKConst.SDK_ADMOB_AD_UNIT_ID_INTERSTITIAL;
            InterstitialAd.Load(adUnitId, CreateAdRequest(), async (InterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null || ad == null)
                {
                    interstitialAdRetryAttempt++;
                    double retryDelay = Math.Pow(2, Math.Min(6, interstitialAdRetryAttempt));
                    await UniTask.Delay(TimeSpan.FromSeconds(retryDelay));
                    if (IsTypeLogEnabled()) Debug.LogError($"==> [AdsManager_Admob] RequestAndLoadInterstitialAd Load Failed With Error: {loadError.GetMessage()}");
                    RequestAndLoadInterstitialAd();
                    return;
                }

                if (IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] RequestAndLoadInterstitialAd Loaded");
                interstitialAd = ad;
                interstitialAdRetryAttempt = 0;

                ad.OnAdFullScreenContentOpened += () => { isShowingAd = true; };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Loom.QueueOnMainThread(() => {
                        interstitialAdCallBack?.Invoke(true);
                    });
                    RequestAndLoadInterstitialAd();
                };
                ad.OnAdImpressionRecorded += () => { };
                ad.OnAdClicked += () => { };
                ad.OnAdFullScreenContentFailed += (AdError error) => 
                { 
                    if(IsTypeLogEnabled()) Debug.LogError($"==> [AdsManager_Admob] IntersititalAd Show Failed With Error: {error.GetMessage()}");
                };
                ad.OnAdPaid += (AdValue adValue) => { };
            });
        }

        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder()
                .AddKeyword("unity-admob-sample")
                .Build();
        }

        
    }
}


