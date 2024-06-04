using UnityEngine;
using Framework;

namespace GameFramework
{
    public class GameStarter : MonoBaseController
    {
        protected override void MonoStart()
        {
            base.MonoStart();
            InitModels();
            InitSystems();

            Input.multiTouchEnabled = false;
            Loom.Initialize();
            CheckForDebug();
        }

        private void InitModels()
        {
            GameApp.Interface.RegisterModel<ISettingModel>(new SettingModel());
        }

        private void InitSystems()
        {

        }

        private void CheckForDebug()
        {
            adsSystem.SetAdsManager(new AdsManager_Editor());
            if (debugSystem.IsDebugFeatureEnabled(EDebugFeature.TimeScaleDown))
                Time.timeScale = 0.2f;
        }
    }
}


