using Cysharp.Threading.Tasks;
using Framework;
using System;
using UnityEngine;

namespace GameFramework
{
    public interface IAdsManager
    {
        void ShowBanner();
        void HideBanner();

        bool IsInterstitialAdReady();
        void ShowInterstitialAd(string place, Action<bool> showCompletedCallback = null);
        bool IsRewardAdReady();
        void ShowRewardAd(string place, Action<bool> showCompletedCallback);

        bool IsApplicationFocusFromAds();
    }

    public interface IAdsSystem : ISystem
    {
        void ShowBanner();
        void HideBanner();
        void CheckShowInterstitialAd(string pos, Action<bool> callBack = null);
        void CheckShowRewardAd(string place, Action<bool> callBack = null);
        void SetAdsManager(IAdsManager adsSystem);
        BindableProperty<bool> InterstitialAdsEnabled { get; } // 插屏，新手保护开关
    }

    public class AdsSystem : AbstractSystem, IAdsSystem, ITypeLog
    {
        public BindableProperty<bool> InterstitialAdsEnabled { get; } = new BindableProperty<bool>() { Value = false };
        private IAdsManager adsManager;
        private IEventSystem eventSystem;
        private int bannerRefreshInterval = 30;

        protected override void OnInit()
        {
            base.OnInit();
            var userRecord = ReadInfoWithReturnNew<AdsSystem>();
            CopyBindableClass(this, userRecord, ()=> SaveInfo(this));
            eventSystem = this.GetSystem<IEventSystem>();

            var isEditor = false;

#if UNITY_EDITOR
            isEditor = true;
#endif

            if (!isEditor) Application.focusChanged += OnApplicationFocus;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus && !adsManager.IsApplicationFocusFromAds())
                CheckShowInterstitialAd("app_focus");
        }

        public bool IsTypeLogEnabled()
        {
            return GameUtils.IsTypeLogEnabled(this);
        }

        public void SetAdsManager(IAdsManager adsManager)
        {
            GameUtils.Log(this, $"{adsManager.GetType().FullName}");
            this.adsManager = adsManager;
        }

        public void CheckShowRewardAd(string place, Action<bool> callBack = null)
        {
            eventSystem.LogAdRewardShowEvent(place);
            if (IsRewardAdReady())
                ShowRewardAd(place, (ret) => {
                    callBack?.Invoke(ret);
                    if(ret)  eventSystem.LogAdRewardShowSuccessEvent(place);
                    else eventSystem.LogAdRewardShowFailedEvent(place);
                });
            else
                CheckLoadMoreRewardAdAsync(place, callBack).Forget();
        }

        public void CheckShowInterstitialAd(string pos, Action<bool> callBack = null)
        {
            if (pos != "AdsTest" && !InterstitialAdsEnabled.Value)
                return;

            eventSystem.LogAdIntShowEvent(pos);
            if (IsInterstitialAdReady())
            {
                eventSystem.LogAdIntShowSuccessEvent(pos);
                ShowInterstitialAd(pos, callBack);
            }
            else
                eventSystem.LogAdIntShowFailedEvent(pos);
        }
     
        private async UniTask CheckLoadMoreRewardAdAsync(string place, Action<bool> callBack = null)
        {
            eventSystem.LogAdRewardLoadEvent(place);
            var languageSystem = this.GetSystem<ILanguageSystem>();
            var uiSystem = this.GetSystem<IUISystem>();

            uiSystem.OpenPopup<AdLoadingPopup>();
            for (int i = 0; i < 5; i++)
            {
                if (IsRewardAdReady())
                {
                    eventSystem.LogAdRewardLoadYesEvent(place);
                    ShowRewardAd(place, (ret) =>
                    {
                        uiSystem.ClosePopup<AdLoadingPopup>();
                        callBack?.Invoke(ret);
                    });
                    return;
                }
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }

            uiSystem.ClosePopup<AdLoadingPopup>();
            eventSystem.LogAdRewardLoadNoEvent(place);
            eventSystem.LogAdRewardLoadReplaceEvent(place);

            if (IsInterstitialAdReady())
            {
                eventSystem.LogAdRewardLoadReplaceYesEvent(place);
                ShowInterstitialAd(place, (ret) => callBack?.Invoke(ret));
            }
            else
            {
                eventSystem.LogAdRewardLoadReplaceNoEvent(place);
                uiSystem.OpenMessage<NormalMessage>(new MessageInfo("message_no_reward_ad"));
            }
        }

        public bool IsRewardAdReady()
        {
            var ret = adsManager != null && adsManager.IsRewardAdReady();
            GameUtils.Log(this, $"ret: {ret}");
            return ret;
        }

        public bool IsInterstitialAdReady()
        {
            var ret = adsManager != null && adsManager.IsInterstitialAdReady();
            GameUtils.Log(this, $"ret: {ret}");
            return ret;
        }

        public void ShowBanner()
        {
            GameUtils.Log(this);
            eventSystem.LogAdBannerShowEvent(bannerRefreshInterval);
            adsManager?.ShowBanner();
        }

        public void HideBanner()
        {
            GameUtils.Log(this);
            adsManager?.HideBanner();
        }

        public void ShowInterstitialAd(string place, Action<bool> showCompletedCallback)
        {
            GameUtils.Log(this, $"place: {place}");
            adsManager?.ShowInterstitialAd(place, showCompletedCallback);
        }

        public void ShowRewardAd(string place, Action<bool> showCompletedCallback)
        {
            GameUtils.Log(this, $"place: {place}");
            adsManager?.ShowRewardAd(place, (ret) => showCompletedCallback?.Invoke(ret));
        }
    }
}

