using UnityEngine;
using Framework;
using IngameDebugConsole;

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
            uiSystem.OpenPopup<DebugPopup>();
            CheckForDebug();
        }

        private void InitModels()
        {
            GameApp.Interface.RegisterModel<ISettingModel>(new SettingModel());
            GameApp.Interface.RegisterModel<IGameModel>(new GameModel());
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
            var isDebugConsoleEnabled = debugModel.IsDebugFeatureEnabled<DebugConsoleEnabled>();
            var debugConsoleGo = GameObject.FindObjectOfType<DebugLogManager>(true);
            debugConsoleGo?.gameObject.SetActive(isDebugConsoleEnabled);

            if (debugModel.IsDebugFeatureEnabled<EditAds>())
                adsSystem.SetAdsManager(new AdsManager_Editor());
        }
    }
}


