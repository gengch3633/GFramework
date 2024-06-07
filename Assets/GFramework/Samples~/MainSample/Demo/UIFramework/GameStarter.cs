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
            GameApp.Interface.RegisterModel<IGameModel>(new GameModel());
            GameApp.Interface.RegisterModel<IStatisticsModel>(new StatisticsModel());
        }

        private void InitSystems()
        {

        }

        private void CheckForDebug()
        {
            adsSystem.SetAdsManager(new AdsManager_Editor());
            if (debugModel.IsDebugFeatureEnabled(EDebugFeature.TimeScaleDown))
                Time.timeScale = 0.2f;
        }
    }
}


