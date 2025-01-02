using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class AdsItem : MonoController, ITypeLog
    {
        private Button btnShowSplashAd;
        private Button btnShowRewardAd;
        private Button btnShowInterstitialAd;
        private Button btnShowBannerAd;
        private Button btnCloseBannerAd;
        private Button btnCloseAdContainer;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            btnShowSplashAd = transform.Find("Viewport/Content/ActionItem/BtnShowSplashAd").GetComponent<Button>();
            btnShowRewardAd = transform.Find("Viewport/Content/ActionItem/BtnShowRewardAd").GetComponent<Button>();
            btnShowInterstitialAd = transform.Find("Viewport/Content/ActionItem/BtnShowInterstitialAd").GetComponent<Button>();
            btnShowBannerAd = transform.Find("Viewport/Content/ActionItem/BtnShowBannerAd").GetComponent<Button>();
            btnCloseBannerAd = transform.Find("Viewport/Content/ActionItem/BtnCloseBannerAd").GetComponent<Button>();
            btnCloseAdContainer = transform.Find("Viewport/Content/ActionItem/BtnCloseAdContainer").GetComponent<Button>();
            btnShowSplashAd.onClick.AddListener(OnBtnShowSplashAdClick);
            btnShowRewardAd.onClick.AddListener(OnBtnShowRewardAdClick);
            btnShowInterstitialAd.onClick.AddListener(OnBtnShowInterstitialAdClick);
            btnShowBannerAd.onClick.AddListener(OnBtnShowBannerAdClick);
            btnCloseBannerAd.onClick.AddListener(OnBtnCloseBannerAdClick);
            btnCloseAdContainer.onClick.AddListener(OnBtnCloseAdContainerClick);
        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();
            btnShowSplashAd.onClick.RemoveListener(OnBtnShowSplashAdClick);
            btnShowRewardAd.onClick.RemoveListener(OnBtnShowRewardAdClick);
            btnShowInterstitialAd.onClick.RemoveListener(OnBtnShowInterstitialAdClick);
            btnShowBannerAd.onClick.RemoveListener(OnBtnShowBannerAdClick);
            btnCloseBannerAd.onClick.RemoveListener(OnBtnCloseBannerAdClick);
            btnCloseAdContainer.onClick.RemoveListener(OnBtnCloseAdContainerClick);
        }
        private void OnBtnShowSplashAdClick()
        {
            GameUtils.Log(this, $"[OnBtnShowSplashAdClick] 1");
            adsSystem.ShowSplashAd();
        }
        private void OnBtnShowRewardAdClick()
        {
            GameUtils.Log(this, $"[OnBtnShowRewardAdClick] 1");
            adsSystem.CheckShowRewardAd("test_video", (success)=> {
                GameUtils.Log(this, $"[OnBtnShowRewardAdClick] 2, Show Result: {success}");
            });
        }
        private void OnBtnShowInterstitialAdClick()
        {
            GameUtils.Log(this, $"[OnBtnShowInterstitialAdClick] 1");
            adsSystem.CheckShowInterstitialAd("AdsTest", (success) => {
                GameUtils.Log(this, $"[OnBtnShowInterstitialAdClick] 2, Show Result: {success}");
            });
        }
        private void OnBtnShowBannerAdClick()
        {
            GameUtils.Log(this, $"[OnBtnShowBannerAdClick] 1");
            adsSystem.ShowBanner();
        }
        private void OnBtnCloseBannerAdClick()
        {
            GameUtils.Log(this, $"[OnBtnCloseBannerAdClick] 1");
            adsSystem.CloseBanner();
        }

        private void OnBtnCloseAdContainerClick()
        {
            GameObject.Destroy(gameObject);
        }

        public bool IsTypeLogEnabled()
        {
            return GameUtils.IsTypeLogEnabled(this);
        }
    }
}

