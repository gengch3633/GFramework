using System.Linq;
using Framework;
using IngameDebugConsole;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public partial class DebugPopup : UIPopup
    {
        private int lv = 1;
        private string password = "qwas";
        private void ChangeDifficultyLevelClick()
        {

        }
        public bool IsTypeLogEnabled()
        {
            return GameUtils.IsTypeLogEnabled(this);
        }
        protected override void OnInitVars()
        {
            base.OnInitVars();
            var languageNames = System.Enum.GetNames(typeof(LanguageType)).ToList();
            var languageIndex = languageNames.IndexOf(languageSystem.GetLanguageType().ToString());
            languageDropdown.ClearOptions();
            languageDropdown.AddOptions(languageNames);
            languageDropdown.value = languageIndex;
            var isDebugSign = debugModel.IsDebugFeatureEnabled<Debug_DebugSign>();
            passWordContainerVar.gameObject.SetActive(!isDebugSign);
        }


        private void TrySetDiffLv(string lvString)
        {
            if(int.TryParse(lvString, out lv))
            {
                uiSystem.OpenMessage<NormalMessage>(new MessageInfo($"TrumpGameBot SetLv: {lv}, Success", true));
            }
        }

        public override void Init(object param = null)
        {
            base.Init(param);

            debugModel.GetAllTypeLogNames().ForEach(typeFullName =>
            {
                var toggleItem = typeLogContainerScrollView.content.CreateItem<ToggleItem>();
                var isOn = debugModel.IsTypeLogEnabled(typeFullName);
                toggleItem.Init(typeFullName, isOn, false);
            });

            debugModel.GetAllDebugFeatureNames().ForEach(debugFeatureName =>
            {
                var toggleItem = debugFeatureContainerScrollView.content.CreateItem<ToggleItem>();
                var isOn = debugModel.IsDebugFeatureEnabled(debugFeatureName);
                toggleItem.Init(debugFeatureName, isOn, true);
            });

            var toggleGroup = toggleDebugFeature.transform.parent.gameObject.AddComponent<ToggleGroup>();
            toggleDebugFeature.group = toggleGroup;
            toggleNormal.group = toggleGroup;
            toggleTypeLog.group = toggleGroup;

            toggleNormal.isOn = true;
        }

        protected override void OnAdListeners()
        {
            base.OnAdListeners();
            this.RegisterEvent<ToggleItemValueChangedEvent>(OnToggleItemValueChangedEvent)
                .UnRegisterWhenGameObjectDestroyed(gameObject);

        }

        private void OnToggleItemValueChangedEvent(ToggleItemValueChangedEvent evt)
        {
            debugModel.SetTypeLogEnabled(evt.itemName, evt.value);
            debugModel.SetDebugFeatureEnabled(evt.itemName, evt.value);
            if (evt.itemName == typeof(Debug_EditorAds).FullName && evt.value)
                adsSystem.SetAdsManager(new AdsManager_Editor());

            if (evt.itemName == typeof(Debug_LogConsole).FullName && evt.value)
            {
                var debugConsoleGo = GameObject.FindObjectOfType<DebugLogManager>(true);
                debugConsoleGo?.gameObject.SetActive(evt.value);
            }
        }
    }
}


