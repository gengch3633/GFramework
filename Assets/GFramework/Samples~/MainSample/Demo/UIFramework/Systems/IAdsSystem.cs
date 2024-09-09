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

        protected override void OnInit()
        {
            base.OnInit();
            var userRecord = ReadInfoWithReturnNew<AdsSystem>();
            CopyBindableClass(this, userRecord, ()=> SaveInfo(this));

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
            var debugSystem = this.GetModel<IDebugModel>();
            var ret = debugSystem.IsTypeLogEnabled(this);
            return ret;
        }

        public void SetAdsManager(IAdsManager adsManager)
        {
            this.adsManager = adsManager;
        }

        public void CheckShowRewardAd(string place, Action<bool> callBack = null)
        {
            EventUtils.LogAdRewardTrigerEvent(place);
            if (IsRewardAdReady())
                ShowRewardAd(place, (ret) => {
                    callBack?.Invoke(ret);
                    //StaticModule.RewardCompleted();
                    //StaticModule.RewardCancel();
                });
            else
            {
                EventUtils.LogAdRewardLoadingEvent();
                //moreRewardADTryCount = 0;
                CheckMoreRewardADState(place, callBack).Forget();
            }
        }

        public void CheckShowInterstitialAd(string pos, Action<bool> callBack = null)
        {
            if (!InterstitialAdsEnabled.Value)
                return;

            EventUtils.LogAdInterstitialTrigerEvent(pos);
            if (IsInterstitialAdReady())
            {
                EventUtils.LogAdInterstitialYesEvent();
                ShowInterstitialAd(pos, callBack);
            }
            else
                EventUtils.LogAdInterstitialNoEvent();
        }
     
        private async UniTask CheckMoreRewardADState(string place, Action<bool> callBack = null)
        {
            var languageSystem = this.GetSystem<ILanguageSystem>();
            var uiSystem = this.GetSystem<IUISystem>();

            //uiSystem.OpenPopup(PopupType.AdLoadingPopup);
            //for (int i = 0; i < 5; i++)
            //{
            //    if (IsRewardAdReady())
            //    {
            //        EventUtils.LogAdRewardLoadingYesEvent();
            //        ShowRewardAd(place, (ret) =>
            //        {
            //            uiSystem.ClosePopup(PopupType.AdLoadingPopup);
            //            callBack?.Invoke(ret);
            //        });
            //        return;
            //    }
            //    await UniTask.Delay(TimeSpan.FromSeconds(1));
            //}

            //uiSystem.ClosePopup(PopupType.AdLoadingPopup);
            EventUtils.LogAdRewardLoadingNoEvent();

            if (IsInterstitialAdReady())
            {
                EventUtils.LogAdRewardLoadingReplaceEvent();
                ShowInterstitialAd(place, (ret) =>
                {
                    callBack?.Invoke(ret);
                });
            }
            else
            {
                EventUtils.LogAdRewardLoadingReplaceNoEvent();
                var info = languageSystem.GetLanguangeText("message_no_reward_ad");
                uiSystem.OpenMessage<NormalMessage>(new MessageInfo(info, true));
            }
        }

        public bool IsRewardAdReady()
        {
            var ret = adsManager != null && adsManager.IsRewardAdReady();
            if(IsTypeLogEnabled()) Debug.LogError($"==> [AdsSystem] [IsRewardAdReady]: {ret}");
            return ret;
        }

        public bool IsInterstitialAdReady()
        {
            var ret = adsManager != null && adsManager.IsInterstitialAdReady();
            if(IsTypeLogEnabled()) Debug.LogError($"==> [AdsSystem] [IsInterstitialAdReady]: {ret}");
            return ret;
        }

        public void ShowBanner()
        {
            if(IsTypeLogEnabled()) Debug.LogError($"==> [AdsSystem] [ShowBanner]");
            adsManager?.ShowBanner();
        }

        public void HideBanner()
        {
            if(IsTypeLogEnabled()) Debug.LogError($"==> [AdsSystem] [CloseBanner]");
            adsManager?.HideBanner();
        }

        public void ShowInterstitialAd(string place, Action<bool> showCompletedCallback)
        {
            if (IsTypeLogEnabled()) Debug.LogError($"==> [AdsSystem] [ShowInterstitialAd]: InterstitialAdsEnabled: {InterstitialAdsEnabled.Value}");

            if(InterstitialAdsEnabled.Value)
                adsManager?.ShowInterstitialAd(place, showCompletedCallback);
        }

        public void ShowRewardAd(string place, Action<bool> showCompletedCallback)
        {
            if(IsTypeLogEnabled()) Debug.LogError($"==> [AdsSystem] [ShowRewardAd]");
            adsManager?.ShowRewardAd(place, (ret) => {
                showCompletedCallback?.Invoke(ret);
                if (ret)
                    EventUtils.LogAdRewardWatchCompletedEvent();
                else
                    EventUtils.LogAdRewardWatchCancelEvent();
            });
        }

        
    }
}

