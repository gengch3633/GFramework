using UnityEngine;
using Framework;
using IngameDebugConsole;
using System;

namespace GameFramework
{
    public class GameStarter : MonoController, ITypeLog
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
            SDKHelper.InitGDPR(this);
        }
       
        void OnApplicationPause(bool pause)
        {
            SDK_Tenjin.OnApplicationPause(pause);
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
           
            var isDebugConsoleEnabled = debugModel.IsDebugFeatureEnabled<Debug_LogConsole>();
            var debugConsoleGo = GameObject.FindObjectOfType<DebugLogManager>(true);
            debugConsoleGo?.gameObject.SetActive(isDebugConsoleEnabled);
        }

        [ContextMenu("Test")]
        private void Test()
        {
            object longT = (int)1;
            object floatT = 1.2f;
            object doubleT = 1.2d;
            object stringT = "111";

            Debug.LogError($"==> longT: {longT}, {longT.GetType().Name}");
            Debug.LogError($"==> floatT: {floatT}, {floatT.GetType().Name}");
            Debug.LogError($"==> doubleT: {doubleT}, {doubleT.GetType().Name}");
            Debug.LogError($"==> stringT: {stringT}, {stringT.GetType().Name}");

            Debug.LogError($"==> long: {typeof(Int32).Name}");
            Debug.LogError($"==> float: {typeof(Single).Name}");
            Debug.LogError($"==> double: {typeof(Double).Name}");
            Debug.LogError($"==> string: {typeof(String).Name}");
        }

        public bool IsTypeLogEnabled()
        {
            return GameUtils.IsTypeLogEnabled(this);
        }
    }
}


