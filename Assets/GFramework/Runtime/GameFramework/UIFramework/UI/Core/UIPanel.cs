using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameFramework
{
    public class UIPanel : UIView
    {
        protected override void OnBtnCloseClickNoAnim()
        {
            uiSystem.ClosePanel(gameObject);
        }

        protected override void OnBtnCloseClick()
        {
            uiSystem.ClosePanel(gameObject);
        }

        protected override async UniTask OnBtnCloseClickAsync()
        {
            uiSystem.ClosePanel(gameObject);
            await OnDestroyAsync();
        }
    }
}

