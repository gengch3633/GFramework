
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
            var debugSystem = GameApp.Interface.GetModel<IDebugModel>();
            var ret = debugSystem.IsTypeLogEnabled("GameFramework.AdsManager_Admob");
            return ret;
        }
        public void Init()
        {
            AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { });
            if (IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] Init 1");
            MobileAds.SetiOSAppPauseOnBackground(true);
            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] Init 2");
            MobileAds.Initialize(HandleInitCompleteAction);
            if(IsTypeLogEnabled()) Debug.LogError("==> [AdsManager_Admob] Init 3");
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
                RequestAndLoadRewardedAd(true);
                RequestAndLoadInterstitialAd(false);
                RequestBannerAd();
            });
        }

        private void RequestAndLoadRewardedAd(bool resetAttemptCount)
        {
            if (resetAttemptCount) rewardedAdRetryAttemptCount = 0;
            LogError($"==> [RequestAndLoadRewardedAd] Load: {rewardedAdRetryAttemptCount}");
            rewardedAd?.Destroy();
            string adUnitId = "";
            RewardedAd.Load(adUnitId, CreateAdRequest(), async (RewardedAd ad, LoadAdError loadError) =>
            {
                if (loadError != null || ad == null)
                {
                    LogError($"[RequestAndLoadRewardedAd] LoadFailed With Error: {loadError.GetMessage()}");
                    double retryDelay = Math.Pow(2, Math.Min(6, rewardedAdRetryAttemptCount));
                    rewardedAdRetryAttemptCount++;
                    await UniTask.Delay(TimeSpan.FromSeconds(retryDelay));
                    RequestAndLoadRewardedAd(false);
                    return;
                }

                LogError($"[RequestAndLoadRewardedAd] Load Success");
                rewardedAd = ad;
                rewardedAdRetryAttemptCount = 0;

                ad.OnAdFullScreenContentOpened += () => { LogError($"[RequestAndLoadRewardedAd] Opened"); };
                ad.OnAdFullScreenContentClosed += () => RequestAndLoadRewardedAd(true);
                ad.OnAdImpressionRecorded += () => { LogError($"[RequestAndLoadRewardedAd] Recorded"); };
                ad.OnAdClicked += () => { LogError($"[RequestAndLoadRewardedAd] Clicked"); };
                ad.OnAdPaid += (AdValue adValue) => { LogError($"[RequestAndLoadRewardedAd] OnAdPaid"); };
                ad.OnAdFullScreenContentFailed += (AdError error) => 
                { 
                    isShowingAd = false;
                    LogError($"[RequestAndLoadRewardedAd] OnAdFullScreenContentFailed Error: {error.GetMessage()}");
                    RequestAndLoadRewardedAd(true);
                };
            });
        }

        private void RequestBannerAd()
        {
            LogError($"[RequestBannerAd] Load");
            bannerView?.Destroy();
            string adUnitId = "";
            bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

            bannerView.OnBannerAdLoaded += ()=> { LogError($"[RequestBannerAd] Loaded"); };
            bannerView.OnBannerAdLoadFailed += (LoadAdError error) => { LogError($"[RequestBannerAd] LoadFailed Error: {error.GetMessage()}"); };
            bannerView.OnAdFullScreenContentOpened += () => { LogError($"[RequestAndLoadRewardedAd] Opened"); };
            bannerView.OnAdFullScreenContentClosed += () => { LogError($"[RequestAndLoadRewardedAd] Closed"); };
            bannerView.OnAdPaid += (AdValue adValue) => { LogError($"[RequestAndLoadRewardedAd] OnAdPaid"); };
            bannerView.OnAdClicked += () => { isShowingAd = true; };

            bannerView.LoadAd(CreateAdRequest());
            HideBanner();
        }

        private void RequestAndLoadInterstitialAd(bool resetAttemptCount)
        {
            if (resetAttemptCount) interstitialAdRetryAttemptCount = 0;
            LogError($"==> [RequestAndLoadInterstitialAd] Load: {resetAttemptCount}");
            interstitialAd?.Destroy();
            string adUnitId = "";
            InterstitialAd.Load(adUnitId, CreateAdRequest(), async (InterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null || ad == null)
                {
                    LogError($"==> [RequestAndLoadInterstitialAd]  LoadFailed With Error: {loadError.GetMessage()}");
                    double retryDelay = Math.Pow(2, Math.Min(6, interstitialAdRetryAttemptCount));
                    interstitialAdRetryAttemptCount++;
                    await UniTask.Delay(TimeSpan.FromSeconds(retryDelay));
                    RequestAndLoadInterstitialAd(false);
                    return;
                }

                LogError("==> [RequestAndLoadInterstitialAd] Loaded");
                interstitialAd = ad;
                interstitialAdRetryAttemptCount = 0;

                ad.OnAdFullScreenContentOpened += () => { LogError($"[RequestAndLoadInterstitialAd] Opened"); };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    LogError($"[RequestAndLoadInterstitialAd] Closed");
                    Loom.QueueOnMainThread(() => {
                        interstitialAdCallBack?.Invoke(true);
                    });
                    RequestAndLoadInterstitialAd(false);
                };
                ad.OnAdImpressionRecorded += () => { LogError($"[RequestAndLoadInterstitialAd] Recorded"); };
                ad.OnAdClicked += () => { LogError($"[RequestAndLoadInterstitialAd] Clicked"); };
                ad.OnAdFullScreenContentFailed += (AdError error) => 
                {
                    isShowingAd = false;
                    LogError($"==> [RequestAndLoadInterstitialAd] OnAdFullScreenContentFailed Error: {error.GetMessage()}");
                    RequestAndLoadInterstitialAd(false);
                };
                ad.OnAdPaid += (AdValue adValue) => { };
            });
        }

        private AdRequest CreateAdRequest()
        {
            return new AdRequest();
        }

        private void LogError(string info)
        {
            if (IsTypeLogEnabled()) Debug.LogError($"==> [{GetType().Name}], {info}");
        }
    }
}


