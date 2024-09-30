
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
#endif
        private Action<bool> interstitialAdCallBack;
        private Action<bool> rewardedAdCallBack;

        private int interstitialAdRetryAttemptCount;
        private int rewardedAdRetryAttemptCount;
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
            var adsConfigs = GameUtils.GetConfigInfos<AdsConfig>();
            var buildTarget = GameUtils.IsIosPlatform() ? BuildTarget.IOS : BuildTarget.Android;
            adsConfig = adsConfigs.Find(item => item.useTestAds == useTestAds && item.buildTarget == buildTarget && item.mediationType == EMediationType.Admob);
            GameUtils.Log(this, $"[AdsManager_Admob] [Release] adsConfig: {JsonConvert.SerializeObject(adsConfig)}");
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

        public void HideBanner()
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
    }
}


