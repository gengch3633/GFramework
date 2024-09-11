
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine;

namespace GameFramework
{
    public partial class AdsManager_Admob : IAdsManager
    {
        private BannerView bannerView;
        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;

        private Action<bool> interstitialAdCallBack;
        private Action<bool> rewardedAdCallBack;

        private int interstitialAdRetryAttemptCount;
        private int rewardedAdRetryAttemptCount;
        private bool isShowingAd = false;
        private AdsConfig adsConfig;
        public AdsManager_Admob()
        {
            GameUtils.Log(this, $"[Create]");
            Application.focusChanged += OnApplicationFocus;
            InitAdsConfig();
            Init();
        }

        private void InitAdsConfig()
        {
            var adsConfigs = GameUtils.GetConfigInfos<AdsConfig>();
            var adsType = GameUtils.IsIosPlatform() ? EAdsType.AdmobIOS : EAdsType.AdmobAndroid;
            var debugModel = GameApp.Interface.GetModel<IDebugModel>();
            if (debugModel.IsDebugFeatureEnabled<Debug_EditorAds>())
                adsType = GameUtils.IsIosPlatform() ? EAdsType.AdmobIOSTest : EAdsType.AdmobAndroidTest;
            adsConfig = adsConfigs.Find(item => item.adsType == adsType);
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
            bannerView?.Show();
        }

        public void HideBanner()
        {
            bannerView?.Hide();
        }

        public bool IsInterstitialAdReady()
        {
            var ret = interstitialAd != null && interstitialAd.CanShowAd();
            return ret;
        }

        public bool IsRewardAdReady()
        {
            var ret = rewardedAd != null && rewardedAd.CanShowAd();
            return ret;
        }

        public void ShowInterstitialAd(string place, Action<bool> showCompletedCallback)
        {
            isShowingAd = true;
            interstitialAdCallBack = showCompletedCallback;
            interstitialAd.Show();
        }

        public void ShowRewardAd(string place, Action<bool> showCompletedCallback)
        {
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
        }
    }
}


