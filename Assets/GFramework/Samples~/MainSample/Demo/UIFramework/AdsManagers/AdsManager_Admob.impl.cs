
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
                RequestAndLoadRewardedAd(false);
                RequestAndLoadInterstitialAd(false);
                RequestAndLoadSplashAd(false);
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

                ad.OnAdFullScreenContentOpened += () => GameUtils.Log(this, $"[RequestAndLoadRewardedAd] [OnAdFullScreenContentOpened]");
                ad.OnAdFullScreenContentClosed += () =>
                {
                    GameUtils.Log(this, $"[RequestAndLoadRewardedAd] [OnAdFullScreenContentClosed]");
                    RequestAndLoadRewardedAd(true);
                };
                
                ad.OnAdImpressionRecorded += () => GameUtils.Log(this, $"[RequestAndLoadRewardedAd] [OnAdImpressionRecorded]");
                ad.OnAdClicked += () => GameUtils.Log(this, $"[RequestAndLoadRewardedAd] [OnAdClicked]"); ;
                ad.OnAdPaid += (AdValue adValue) => GameUtils.Log(this, $"[RequestAndLoadRewardedAd] [OnAdPaid], adValue.Value: {adValue.Value}, adValue.CurrencyCode: {adValue.CurrencyCode}"); ;
                ad.OnAdFullScreenContentFailed += (AdError error) => 
                { 
                    isShowingAd = false;
                    GameUtils.Log(this, $"[RequestAndLoadRewardedAd] [OnAdFullScreenContentFailed], Error: {error.GetMessage()}");
                    RequestAndLoadRewardedAd(true);
                };
            });
        }

        public void RequestAndLoadSplashAd(bool resetAttemptCount)
        {
            if (resetAttemptCount) splashAdRetryAttemptCount = 0;
            if (splashAd != null)
            {
                splashAd.Destroy();
                splashAd = null;
            }
            GameUtils.Log(this, $"[RequestAndLoadSplashAd] Load");
            var adRequest = new AdRequest();
            AppOpenAd.Load(adsConfig.splashAdId, adRequest, async (AppOpenAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    GameUtils.Log(this, $"[RequestAndLoadSplashAd], LoadFailed With Error: {error.GetMessage()}");
                    double retryDelay = Math.Pow(2, Math.Min(6, splashAdRetryAttemptCount));
                    splashAdRetryAttemptCount++;
                    await UniTask.Delay(TimeSpan.FromSeconds(retryDelay));
                    RequestAndLoadSplashAd(false);
                    return;
                }
                GameUtils.Log(this, $"[RequestAndLoadSplashAd] Loaded, Message: {ad.GetResponseInfo()}");
                splashAd = ad;
                ad.OnAdPaid += (AdValue adValue) => GameUtils.Log(this, $"[RequestAndLoadSplashAd] [OnAdPaid], adValue.Value: {adValue.Value}, adValue.CurrencyCode: {adValue.CurrencyCode}");
                ad.OnAdImpressionRecorded += () => GameUtils.Log(this, $"[RequestAndLoadSplashAd] [OnAdImpressionRecorded]");
                ad.OnAdClicked += () => GameUtils.Log(this, $"[RequestAndLoadSplashAd] [OnAdClicked]");
                ad.OnAdFullScreenContentOpened += () => GameUtils.Log(this, $"[RequestAndLoadSplashAd] [OnAdFullScreenContentOpened]");
                ad.OnAdFullScreenContentClosed += () =>
                {
                    GameUtils.Log(this, $"[RequestAndLoadSplashAd] [OnAdFullScreenContentClosed]");
                    RequestAndLoadSplashAd(true);
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    isShowingAd = false;
                    GameUtils.Log(this, $"[RequestAndLoadSplashAd] [OnAdFullScreenContentFailed], error: {error}");
                    RequestAndLoadSplashAd(true);
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

                ad.OnAdFullScreenContentOpened += () => GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] [OnAdFullScreenContentOpened]");
                ad.OnAdFullScreenContentClosed += () =>
                {
                    GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] [OnAdFullScreenContentClosed]");
                    Loom.QueueOnMainThread(() => {
                        interstitialAdCallBack?.Invoke(true);
                    });
                    RequestAndLoadInterstitialAd(true);
                };
                ad.OnAdImpressionRecorded += () => GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] [OnAdImpressionRecorded]");
                ad.OnAdClicked += () => GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] [OnAdClicked]"); ;
                ad.OnAdFullScreenContentFailed += (AdError error) => 
                {
                    isShowingAd = false;
                    GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] [OnAdFullScreenContentFailed], Error: {error.GetMessage()}");
                    RequestAndLoadInterstitialAd(true);
                };
                ad.OnAdPaid += (AdValue adValue) => GameUtils.Log(this, $"[RequestAndLoadInterstitialAd] [OnAdPaid], adValue.Value: {adValue.Value}, adValue.CurrencyCode: {adValue.CurrencyCode}");
            });
        }

        private void RequestBannerAd()
        {
            GameUtils.Log(this, $"[RequestBannerAd] Load");
            bannerView?.Destroy();
            bannerView = new BannerView(adsConfig.bannerAdId, AdSize.Banner, AdPosition.Bottom);

            bannerView.OnBannerAdLoaded += () => GameUtils.Log(this, $"[RequestBannerAd] [OnBannerAdLoaded]"); UpdateBannerHeight();
            bannerView.OnBannerAdLoadFailed += (LoadAdError error) => GameUtils.Log(this, $"[RequestBannerAd] [OnBannerAdLoadFailed], Error: {error.GetMessage()}");
            bannerView.OnAdFullScreenContentOpened += () => GameUtils.Log(this, $"[RequestBannerAd] [OnAdFullScreenContentOpened]");
            bannerView.OnAdFullScreenContentClosed += () => GameUtils.Log(this, $"[RequestBannerAd] [OnAdFullScreenContentClosed]");
            bannerView.OnAdPaid += (AdValue adValue) => GameUtils.Log(this, $"[RequestBannerAd] [OnAdPaid], adValue.Value: {adValue.Value}, adValue.CurrencyCode: {adValue.CurrencyCode}");
            bannerView.OnAdClicked += () => 
            {
                GameUtils.Log(this, $"[RequestAndLoadRewardedAd] [OnAdClicked]");
                isShowingAd = true; 
            };

            bannerView.LoadAd(CreateAdRequest());
            CloseBanner();
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