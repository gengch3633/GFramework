
using Cysharp.Threading.Tasks;
#if SDK_ADMOB
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Newtonsoft.Json;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public partial class AdsManager_Admob: ITypeLog
    {
        public bool IsTypeLogEnabled()
        {
            return GameUtils.IsTypeLogEnabled(this);
        }
        public void Init()
        {
#if SDK_ADMOB
            AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { });
            GameUtils.Log(this, "1");
            MobileAds.SetiOSAppPauseOnBackground(true);
            GameUtils.Log(this, "2");
            MobileAds.Initialize(HandleInitCompleteAction);
            GameUtils.Log(this, "3");
#endif
        }
#if SDK_ADMOB
        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            GameUtils.Log(this, "1");
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
                GameUtils.Log(this, $"2, {className}: {status.InitializationState}");
            }

            GameUtils.Log(this, "3");
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
            GameUtils.Log(this, $"[RequestAndLoadRewardedAd] Load, AttemptCount: {rewardedAdRetryAttemptCount}");
            rewardedAd?.Destroy();
            RewardedAd.Load(adsConfig.rewardAdId, CreateAdRequest(), async (RewardedAd ad, LoadAdError loadError) =>
            {
                if (loadError != null || ad == null)
                {
                    GameUtils.Log(this, $"[RequestAndLoadRewardedAd], LoadFailed With Error: {loadError.GetMessage()}");
                    double retryDelay = Math.Pow(2, Math.Min(6, rewardedAdRetryAttemptCount));
                    rewardedAdRetryAttemptCount++;
                    await UniTask.Delay(TimeSpan.FromSeconds(retryDelay));
                    RequestAndLoadRewardedAd(false);
                    return;
                }
                GameUtils.Log(this, $"[RequestAndLoadRewardedAd] Loaded");

                rewardedAd = ad;
                rewardedAdRetryAttemptCount = 0;

                ad.OnAdFullScreenContentOpened += () => { GameUtils.Log(this, $"[RequestAndLoadRewardedAd] Opened"); };
                ad.OnAdFullScreenContentClosed += () => RequestAndLoadRewardedAd(true);
                ad.OnAdImpressionRecorded += () => { GameUtils.Log(this, $"[RequestAndLoadRewardedAd] Recorded"); };
                ad.OnAdClicked += () => { GameUtils.Log(this, $"[RequestAndLoadRewardedAd] Clicked"); };
                ad.OnAdPaid += (AdValue adValue) => { GameUtils.Log(this, $"[RequestAndLoadRewardedAd] OnAdPaid"); };
                ad.OnAdFullScreenContentFailed += (AdError error) => 
                { 
                    isShowingAd = false;
                    GameUtils.Log(this, $"[RequestAndLoadRewardedAd] OnAdFullScreenContentFailed Error: {error.GetMessage()}");
                    RequestAndLoadRewardedAd(true);
                };
            });
        }

        private void RequestAndLoadInterstitialAd(bool resetAttemptCount)
        {
            if (resetAttemptCount) interstitialAdRetryAttemptCount = 0;
            GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] Load, AttemptCount: {interstitialAdRetryAttemptCount}");
            interstitialAd?.Destroy();
            InterstitialAd.Load(adsConfig.interstitialAdId, CreateAdRequest(), async (InterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null || ad == null)
                {
                    GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] LoadFailed With Error: {loadError.GetMessage()}");
                    double retryDelay = Math.Pow(2, Math.Min(6, interstitialAdRetryAttemptCount));
                    interstitialAdRetryAttemptCount++;
                    await UniTask.Delay(TimeSpan.FromSeconds(retryDelay));
                    RequestAndLoadInterstitialAd(false);
                    return;
                }

                GameUtils.Log(this, "[RequestAndLoadInterstitialAd] Loaded");
                interstitialAd = ad;
                interstitialAdRetryAttemptCount = 0;

                ad.OnAdFullScreenContentOpened += () => { GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] Opened"); };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] Closed");
                    Loom.QueueOnMainThread(() => {
                        interstitialAdCallBack?.Invoke(true);
                    });
                    RequestAndLoadInterstitialAd(false);
                };
                ad.OnAdImpressionRecorded += () => { GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] Recorded"); };
                ad.OnAdClicked += () => { GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] Clicked"); };
                ad.OnAdFullScreenContentFailed += (AdError error) => 
                {
                    isShowingAd = false;
                    GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] OnAdFullScreenContentFailed Error: {error.GetMessage()}");
                    RequestAndLoadInterstitialAd(false);
                };
                ad.OnAdPaid += (AdValue adValue) => { };
            });
        }

        private void RequestBannerAd()
        {
            GameUtils.Log(this, $"[RequestBannerAd] Load");
            bannerView?.Destroy();
            bannerView = new BannerView(adsConfig.bannerAdId, AdSize.Banner, AdPosition.Bottom);

            bannerView.OnBannerAdLoaded += () => { GameUtils.Log(this, $"[RequestBannerAd] Loaded"); UpdateBannerHeight(); };
            bannerView.OnBannerAdLoadFailed += (LoadAdError error) => { GameUtils.Log(this, $"[RequestBannerAd] LoadFailed Error: {error.GetMessage()}"); };
            bannerView.OnAdFullScreenContentOpened += () => { GameUtils.Log(this, $"[RequestAndLoadRewardedAd] Opened"); };
            bannerView.OnAdFullScreenContentClosed += () => { GameUtils.Log(this, $"[RequestAndLoadRewardedAd] Closed"); };
            bannerView.OnAdPaid += (AdValue adValue) => { GameUtils.Log(this, $"[RequestAndLoadRewardedAd] OnAdPaid"); };
            bannerView.OnAdClicked += () => { isShowingAd = true; };

            bannerView.LoadAd(CreateAdRequest());
            HideBanner();
        }

        public void UpdateBannerHeight()
        {
            var userModel = GameApp.Interface.GetModel<IUserModel>();
            var bannerPixelHeight = bannerView.GetHeightInPixels();
            var heightPx = Screen.height;
            var heightPxRatio = bannerPixelHeight / heightPx;

            var canvasScaler = GameObject.Find("UISystem/Bottom").GetComponent<CanvasScaler>();
            var bannerHeight = canvasScaler.referenceResolution.y * heightPxRatio;
            userModel.BannerHeight.Value = bannerHeight;
            GameUtils.Log(this, $"canvasScaler: {canvasScaler.gameObject.name}", canvasScaler.gameObject);
            GameUtils.Log(this, $"settingModel.BannerHeight.Value: {userModel.BannerHeight.Value}");
        }

        private AdRequest CreateAdRequest()
        {
            return new AdRequest();
        }
#endif
    }
}


