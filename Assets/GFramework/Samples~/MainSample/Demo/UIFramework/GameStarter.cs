using UnityEngine;
using Framework;
using IngameDebugConsole;

namespace GameFramework
{
    public class GameStarter : MonoController
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
            GameApp.Interface.RegisterSystem<IEventSystem>(new EventSystem());
            GameApp.Interface.RegisterSystem<IAdsSystem>(new AdsSystem());
        }

        private void CheckForDebug()
        {
            adsSystem = this.GetSystem<IAdsSystem>();
            var isDebugConsoleEnabled = debugModel.IsDebugFeatureEnabled<Debug_LogConsole>();
            var debugConsoleGo = GameObject.FindObjectOfType<DebugLogManager>(true);
            debugConsoleGo?.gameObject.SetActive(isDebugConsoleEnabled);

            if (!debugModel.IsDebugFeatureEnabled<Debug_EditorAds>())
                adsSystem.SetAdsManager(new AdsManager_Admob());
        }
    }
}


