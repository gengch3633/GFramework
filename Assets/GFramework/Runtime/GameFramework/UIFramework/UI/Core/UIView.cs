using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class UIView : MonoBaseController
    {
        public virtual void SetUISortingOrder(GameObject go, int order)
        {
            var parentCanvas = transform.parent.GetComponent<Canvas>();
            var canvas = go.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = go.AddComponent<Canvas>();
                if (go.GetComponent<GraphicRaycaster>() == null)
                    go.AddComponent<GraphicRaycaster>();
            }
            canvas.overrideSorting = true;
            canvas.sortingOrder = parentCanvas.sortingOrder + order;

            //Debug.LogError("==> SetUISortingOrder: " + go.name + ", " + order, go);
        }

        public virtual void Init(object param = null)
        {

        }

        protected virtual void OnOpenAnimCompleteEvent()
        {

        }

        protected virtual void OnBtnCloseClickNoAnim()
        {

        }

        protected virtual void OnBtnCloseClick()
        {

        }

        protected virtual async UniTask OnBtnCloseClickAsync()
        {
            await UniTask.DelayFrame(1);
        }

     
        private bool isDestroyed = false;
        private object returnAsyncObject = null;
        protected override void OnDestroy()
        {
            base.OnDestroy();
            isDestroyed = true;
        }

        public void SetReturnAsyncObject(object obj)
        {
            this.returnAsyncObject = obj;
        }

        public async UniTask<object> OnDestroyAsyncObject()
        {
            await UniTask.WaitUntil(() => isDestroyed);
            return returnAsyncObject;
        }


        public async UniTask OnDestroyAsync()
        {
            await UniTask.WaitUntil(() => isDestroyed);
        }
    }
}

