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
            uiSystem.OpenPopup<DebugPopup>();
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

        void OnApplicationQuit()
        {
            SaveInfo();
        }

        void OnApplicationPause(bool pause)
        {
            if (pause) SaveInfo();
        }

        private void SaveInfo()
        {
            GameApp.Interface.Systems.ForEach(item => item.SaveInfo());
            GameApp.Interface.Models.ForEach(item => item.SaveInfo());
        }

        private void CheckForDebug()
        {
            adsSystem.SetAdsManager(new AdsManager_Editor());
            if (debugModel.IsDebugFeatureEnabled(EDebugFeature.TimeScaleDown))
                Time.timeScale = 0.2f;
        }
    }
}


