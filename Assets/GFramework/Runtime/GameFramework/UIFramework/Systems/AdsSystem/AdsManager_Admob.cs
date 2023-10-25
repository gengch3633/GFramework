
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

        private int interstitialAdRetryAttempt;
        private int rewardedAdRetryAttempt;
        private bool isShowingAd = false;

        public AdsManager_Admob()
        {
            Init();

            Application.focusChanged += OnApplicationFocus;
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
            interstitialAdCallBack = showCompletedCallback;
            interstitialAd.Show();
        }

        public void ShowRewardAd(string place, Action<bool> showCompletedCallback)
        {
            if (IsTypeLogEnabled()) Debug.LogError("==> [ShowRewardAd] Start");
            rewardedAdCallBack = showCompletedCallback;
            rewardedAd.Show((Reward reward) =>
            {
                Loom.QueueOnMainThread(() =>
                {
                    if (IsTypeLogEnabled()) Debug.LogError("==> [ShowRewardAd] Rewarded");
                    rewardedAdCallBack?.Invoke(true);
                });
            });
        }
    }
}

