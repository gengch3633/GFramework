
using Cysharp.Threading.Tasks;
#if SDK_ADMOB
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
#endif
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace GameFramework
{
    public partial class AdsManager_Admob : IAdsManager
    {
#if SDK_ADMOB
        private BannerView bannerView;
        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;
        private AppOpenAd splashAd;
#endif
        private Action<bool> interstitialAdCallBack;
        private Action<bool> rewardedAdCallBack;

        private int interstitialAdRetryAttemptCount;
        private int rewardedAdRetryAttemptCount;
        private int splashAdRetryAttemptCount;
        private bool isShowingAd = false;
        private AdsConfig adsConfig;
        public AdsManager_Admob()
        {
#if SDK_ADMOB
            GameUtils.Log(this, $"[Create]");
            Application.focusChanged += OnApplicationFocus;
            InitAdsConfig();
            Init();
#endif
        }
        private void InitAdsConfig()
        {
            var debugModel = GameApp.Interface.GetModel<IDebugModel>();
            var useTestAds = debugModel.IsDebugFeatureEnabled<Debug_TestAds>();
            var releaseAdsConfigs = GameUtils.GetConfigInfos<AdsConfig>();
            var testAdsConfigs = GameUtils.GetConfigInfos<AdsConfig>("Test");
            var buildTarget = GameUtils.IsIosPlatform() ? BuildTarget.IOS : BuildTarget.Android;
            adsConfig = releaseAdsConfigs.Find(item => item.buildTarget == buildTarget && item.mediationType == EMediationType.Admob);
            GameUtils.Log(this, $"[AdsManager_Admob] [Release] adsConfig 1: {JsonConvert.SerializeObject(adsConfig)}");
            if (useTestAds)
            {
                var testAdsConfig = testAdsConfigs.Find(item => item.buildTarget == buildTarget && item.mediationType == EMediationType.Admob);
                adsConfig.SetAdsIds(testAdsConfig);
                GameUtils.Log(this, $"[AdsManager_Admob] [Release] adsConfig 2: {JsonConvert.SerializeObject(testAdsConfig)}");
                GameUtils.Log(this, $"[AdsManager_Admob] [Release] testAdsConfig 3: {JsonConvert.SerializeObject(adsConfig)}");
            }

            GameUtils.Log(this, $"[AdsManager_Admob] [Release] adsConfig 4 End: {JsonConvert.SerializeObject(adsConfig)}");
        }

        private async void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
                isShowingAd = false;
            }
        }

        public bool IsApplicationFocusFromAds()
        {
            return isShowingAd;
        }

        public void ShowBanner()
        {
#if SDK_ADMOB
            bannerView?.Show();
#endif
        }

        public void CloseBanner()
        {
#if SDK_ADMOB
            bannerView?.Hide();
#endif
        }

        public bool IsInterstitialAdReady()
        {
#if SDK_ADMOB
            var ret = interstitialAd != null && interstitialAd.CanShowAd();
            return ret;
#else
            return false;
#endif
        }

        public bool IsRewardAdReady()
        {
#if SDK_ADMOB
            var ret = rewardedAd != null && rewardedAd.CanShowAd();
            return ret;
#else
            return false;
#endif
        }

        public void ShowInterstitialAd(string place, Action<bool> showCompletedCallback)
        {
#if SDK_ADMOB
            isShowingAd = true;
            interstitialAdCallBack = showCompletedCallback;
            interstitialAd.Show();
#endif
        }

        public void ShowRewardAd(string place, Action<bool> showCompletedCallback)
        {
#if SDK_ADMOB
            isShowingAd = true;
            GameUtils.Log(this, "[ShowRewardAd] Start");
            rewardedAdCallBack = showCompletedCallback;
            rewardedAd.Show((Reward reward) =>
            {
                Loom.QueueOnMainThread(() =>
                {
                    GameUtils.Log(this, "[ShowRewardAd] Rewarded");
                    rewardedAdCallBack?.Invoke(true);
                });
            });
#endif
        }

        public bool IsSplashAdReady()
        {
#if SDK_ADMOB
            var ret = splashAd != null && splashAd.CanShowAd();
            return ret;
#else
            return false;
#endif
        }

        public void ShowSplashAd(string place = "default")
        {
#if SDK_ADMOB
            isShowingAd = true;
            GameUtils.Log(this, "[ShowSplashAd] Start");
            splashAd.Show();
#endif
        }
    }
}


