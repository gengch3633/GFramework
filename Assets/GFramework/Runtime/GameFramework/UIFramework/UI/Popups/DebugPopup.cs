using Framework;
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
                uiSystem.OpenMessage(MessageType.NormalMessage, new MessageInfo($"TrumpGameBot SetLv: {lv}, Success", true));
            }
        }

        public override void Init(object param = null)
        {
            base.Init(param);

            debugSystem.GetAllTypeLogNames().ForEach(typeFullName =>
            {
                var toggleItem = typeLogContainerScrollView.content.CreateItem<ToggleItem>();
                var isOn = debugSystem.IsTypeLogEnabled(typeFullName);
                toggleItem.Init(typeFullName, isOn);
            });

            debugSystem.GetAllDebugFeatureNames().ForEach(debugFeatureName =>
            {
                var toggleItem = debugFeatureContainerScrollView.content.CreateItem<ToggleItem>();
                var isOn = debugSystem.IsDebugFeatureEnabled(debugFeatureName);
                toggleItem.Init(debugFeatureName, isOn);
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
            debugSystem.SetTypeLogEnabled(evt.itemName, evt.value);
            debugSystem.SetDebugFeatureEnabled(evt.itemName, evt.value);

            if (evt.itemName == EDebugFeature.NoAds.ToString())
                adsSystem.SetAdsManager(new AdsManager_Editor());

            if (evt.itemName == EDebugFeature.TimeScaleDown.ToString())
            {
                var isOn = evt.value;
                var timeScale = isOn ? 0.2f : 1.0f;
                Time.timeScale = timeScale;
            }
        }
    }
}


