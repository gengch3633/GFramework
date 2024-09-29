
using UnityEngine.UI;
using Framework;
using Cysharp.Threading.Tasks;
using System;

namespace GameFramework
{
    public class ToggleItem : MonoBaseController
    {
        private string lableString;
        public void Init(string lableString, bool isOn, bool isDebugFeature)
        {
            this.lableString = lableString;
            lableTextVar.text = lableString;
            if (isDebugFeature)
                toggleItem.isOn = isOn;
            else
                toggleItem.SetIsOnWithoutNotify(isOn);
        }

        private Toggle toggleItem;
        private Text lableTextVar;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            toggleItem = transform.Find("").GetComponent<Toggle>();
            lableTextVar = transform.Find("LableText_Var").GetComponent<Text>();
            toggleItem.onValueChanged.AddListener(OnToggleItemChanged);
        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();
            toggleItem.onValueChanged.RemoveListener(OnToggleItemChanged);
        }
        private void OnToggleItemChanged(bool value) 
        {
            debugModel.SendEvent(new ToggleItemValueChangedEvent(lableString, value));
        }
    }
}

