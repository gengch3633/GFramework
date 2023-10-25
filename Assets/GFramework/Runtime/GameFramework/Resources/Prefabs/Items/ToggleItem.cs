
using UnityEngine.UI;
using Framework;

namespace GameFramework
{
    public class ToggleItem : MonoController
    {
        private string lableString;
        public void Init(string lableString, bool isOn)
        {
            this.lableString = lableString;
            lableTextVar.text = lableString;
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
            gameModel.SendEvent(new ToggleItemValueChangedEvent(lableString, value));
        }
    }
}

