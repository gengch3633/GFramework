

using UnityEngine;

namespace GameFramework
{
    public class GameStarter : MonoController
    {
        void Start()
        {
            //½ûÓÃ¶àµã´¥Ãþ
            Input.multiTouchEnabled = false;

            Loom.Initialize();
            CheckForDebug();
            uiSystem.OpenPanel(PanelType.LoadingPanel);
        }

        private void CheckForDebug()
        {
            if (debugSystem.IsDebugFeatureEnabled(EDebugFeature.NoAds))
                adsSystem.SetAdsManager(new AdsManager_Editor());
            else
            {
                var isProdutionAds = !debugSystem.IsDebugFeatureEnabled(EDebugFeature.MobileTestAds);
                adsSystem.SetAdsManager(new AdsManager_Admob(isProdutionAds));
            }

            if (debugSystem.IsDebugFeatureEnabled(EDebugFeature.TimeScaleDown))
                Time.timeScale = 0.2f;
        }
    }
}


