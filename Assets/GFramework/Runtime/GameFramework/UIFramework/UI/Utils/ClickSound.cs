
using UnityEngine.UI;

namespace GameFramework
{
    public class ClickSound : MonoController
    {
        public string soundName = "buttons";
        private void OnBtnClick()
        {
            audioSystem.PlaySound(soundName);
        }

        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            GetComponent<Button>()?.onClick.AddListener(OnBtnClick);
        }

        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();
            GetComponent<Button>()?.onClick.RemoveListener(OnBtnClick);
        }
    }
}

