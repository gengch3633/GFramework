using Cysharp.Threading.Tasks;
using Framework;
using System;
using UnityEngine;

namespace GameFramework
{
    public class UIBasePopup : UIView
    {
        public string popupOpenSound = "popup_open";
        public string popupCloseSound = "popup_close";
        public string popupCloseSoundNoAnim = "popup_close_no_anim";
        public bool usePopupAdapter = false;
        public override async void Init(object param = null)
        {
            base.Init(param);
            if (usePopupAdapter) gameObject.AddComponent<PopupAdapter>();
            await OnOpenPopupAsync();
        }

        private async UniTask OnOpenPopupAsync()
        {
            UIPopAnim popAnim = GetComponent<UIPopAnim>();
            var waitFrameCount = userModel.OpenPopupAnimWaitFrameCount.Value;
            if (waitFrameCount > 0)
            {
                popAnim.ResetPopinAim();
                await UniTask.DelayFrame(waitFrameCount);
            }
            audioSystem.PlaySound(popupOpenSound);
            if (popAnim != null)
                popAnim.PlayPopinAim(OnOpenAnimCompleteEvent);
            else
            {
                await PlayOpenPopAnim();
                OnOpenAnimCompleteEvent();
            }
        }

        public async UniTask OnClosePopupAsync(bool showCloseAnim, Action onComplete = null)
        {
            
            UIPopAnim popAnim = GetComponent<UIPopAnim>();
            if (popAnim != null && showCloseAnim)
            {
                audioSystem.PlaySound(popupCloseSound);
                popAnim.PlayPopoutAim(() => uiSystem.OnCloseView(gameObject, onComplete));
            }
            else
            {
                await PlayClosePopAnim();
                audioSystem.PlaySound(popupCloseSoundNoAnim);
                uiSystem.OnCloseView(gameObject, onComplete);
            }
        }

        protected virtual async UniTask PlayOpenPopAnim()
        {
            await UniTask.DelayFrame(1);
        }

        protected virtual async UniTask PlayClosePopAnim()
        {
            await UniTask.DelayFrame(1);
        }

        protected override void OnBtnCloseClickNoAnim()
        {
            CloseSelfPopup(showCloseAnim: false);
        }

        protected override void OnBtnCloseClick()
        {
            CloseSelfPopup();
        }

        protected override async UniTask OnBtnCloseClickAsync()
        {
            CloseSelfPopup();
            await OnDestroyAsync();
        }

        private void CloseSelfPopup(Action onComplete = null, bool showCloseAnim = true)
        {
            uiSystem.ClosePopup(gameObject, onComplete, showCloseAnim);
        }
    }
}

