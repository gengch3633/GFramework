

using UnityEngine;

namespace GameFramework
{
    public class GameStarter : MonoBaseController
    {
        void Start()
        {
            //½ûÓÃ¶àµã´¥Ãþ
            Input.multiTouchEnabled = false;

            Loom.Initialize();
            CheckForDebug();
            uiSystem.OpenPopup<DebugPopup>();
        }

        private void CheckForDebug()
        {
            adsSystem.SetAdsManager(new AdsManager_Editor());
            if (debugSystem.IsDebugFeatureEnabled(EDebugFeature.TimeScaleDown))
                Time.timeScale = 0.2f;
        }
    }
}


