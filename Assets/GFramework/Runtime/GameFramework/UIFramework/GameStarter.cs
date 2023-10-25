

using UnityEngine;

namespace GameFramework
{
    public class GameStarter : MonoController
    {
        void Start()
        {
            //���ö�㴥��
            Input.multiTouchEnabled = false;

            Loom.Initialize();
            CheckForDebug();
            uiSystem.OpenPanel(PanelType.LoadingPanel);
        }

        private void CheckForDebug()
        {
            if (debugSystem.IsDebugFeatureEnabled(EDebugFeature.NoAdsEditor))
                adsSystem.SetAdsManager(new AdsManager_Editor());
            else
                adsSystem.SetAdsManager(new AdsManager_Admob());

            if (debugSystem.IsDebugFeatureEnabled(EDebugFeature.TimeScaleDown))
                Time.timeScale = 0.2f;
        }
    }
}


