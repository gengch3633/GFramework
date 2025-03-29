using System;
using System.Collections;
using System.Collections.Generic;
#if SDK_ADMOB
using GoogleMobileAds.Ump.Api;
#endif
using UnityEngine;

namespace GameFramework
{
    public class SDKHelper 
    {
        public static void InitGDPR(GameStarter gameStarter)
        {
#if SDK_ADMOB
            var debugModel = GameApp.Interface.GetModel<IDebugModel>();
            if (!debugModel.IsDebugFeatureEnabled<Debug_NoGDPR>())
            {
                var uiSystem = GameApp.Interface.GetSystem<IUISystem>();
                var gdprPopup = uiSystem.OpenPopup<GDPRPopup>();
                gdprPopup.GatherConsent((error) =>
                {
                    GameUtils.Log(gameStarter, $"InitGDPR 1, error: {error}, ConsentInformation.ConsentStatus: {ConsentInformation.ConsentStatus}");
                    GameUtils.Log(gameStarter, $"InitGDPR 2, GDPRPopup.CanRequestAds: {GDPRPopup.CanRequestAds}");

                    if (GDPRPopup.CanRequestAds)
                    {
                        InitSdk();
                        RequestAuthorizationTracking();
                        uiSystem.ClosePopup<GDPRPopup>();
                    }
                });
            }
            else
                InitSdk();
#else
            InitSdk();
#endif

        }
        private static void InitSdk()
        {
            var eventSystem = GameApp.Interface.GetSystem<IEventSystem>();
            eventSystem.AddEventTracker(new EventTracker_TD());
            eventSystem.AddEventTracker(new EventTracker_Firebase());
            SDK_Tenjin.Init();
            SDK_FB.Init(); 
            var adsSystem = GameApp.Interface.GetSystem<IAdsSystem>();
            var debugModel = GameApp.Interface.GetModel<IDebugModel>();
            if (!debugModel.IsDebugFeatureEnabled<Debug_EditorAds>())
                adsSystem.SetAdsManager(new AdsManager_Admob());
            else
                adsSystem.SetAdsManager(new AdsManager_Editor());
        }
        public static void RequestAuthorizationTracking()
        {
#if UNITY_IOS
            Debug.LogError($"===> InitGDPR 5");
            Debug.Log("Unity iOS Support: Requesting iOS App Tracking Transparency native dialog.");

            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            Version currentVersion = new Version("16.0");
            if (!Application.isEditor)
            {
                currentVersion = new Version(Device.systemVersion);
            }
            Debug.LogError($"===> InitGDPR currentVersion:{currentVersion}, AuthorizationTrackingStatus:{status}");
            Version ios14 = new Version("14.5"); 

            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED && currentVersion >= ios14)
            {
                Debug.LogError($"===> InitGDPR 6");
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }

            Debug.LogError($"===> InitGDPR 7");
#else
            Debug.LogWarning("Unity iOS Support: Tried to request iOS App Tracking Transparency native dialog, " +
                             "but the current platform is not iOS.");
#endif
        }
    }
}

