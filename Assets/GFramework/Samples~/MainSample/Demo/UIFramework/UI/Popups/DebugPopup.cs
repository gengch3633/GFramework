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
        private void ChangeDifficultyLevelClick()
        {
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

            toggleDebugFeature.isOn = true;
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
            if (evt.itemName == typeof(EditAds).FullName)
                adsSystem.SetAdsManager(new AdsManager_Editor());
            else
                adsSystem.SetAdsManager(new AdsManager_Editor());

            if (evt.itemName == typeof(DebugConsoleEnabled).FullName)
            {
                var debugConsoleGo = GameObject.FindObjectOfType<DebugLogManager>(true);
                debugConsoleGo?.gameObject.SetActive(evt.value);
            }
        }
    }
}


