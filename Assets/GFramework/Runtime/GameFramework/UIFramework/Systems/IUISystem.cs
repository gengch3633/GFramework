using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace GameFramework
{
    public interface IUISystem : ISystem
    {
        void CloseAllMessages();
        void ClosePanel(GameObject panel, Action onComplete = null);
        void ClosePanel(PanelType panelType, Action onComplete = null);
        void ClosePopup(PopupType popupType, Action onComplete = null, bool showCloseAnim = true);
        void ClosePopup(GameObject popup, Action onComplete, bool showCloseAnim);
        bool HasPopup(PopupType popup);
        void OnCloseView(GameObject popup, Action onComplete = null);
        void OpenMessage(MessageType messageType, MessageInfo messageInfo);
        void OpenPanel(PanelType panelType, object param = null);
        UIPopup OpenPopup(PopupType popupType, object param = null);
        UniTask OpenPopupAsync(PopupType popupType, object param = null);
        UniTask<bool> OpenPopupAsyncBool(PopupType popupType, object param = null);
        UniTask<object> OpenPopupAsyncObject(PopupType popupType, object param = null);
        void SetInteractable(bool isInteractable);
        bool IsInteractable();
    }
    public class UISystem: AbstractSystem, IUISystem, ITypeLog
    {
        private Transform uiSystemTransform;
        private Transform bottomLayer;
        private Transform middleLayer;
        private Transform topLayer;

        private Dictionary<GameObject, PopupType> popupDict = new Dictionary<GameObject, PopupType>();
        private Dictionary<GameObject, PanelType> panelDict = new Dictionary<GameObject, PanelType>();
        private bool isInteractable = true;

        public bool IsTypeLogEnabled()
        {
            var debugSystem = this.GetSystem<IDebugSystem>();
            return debugSystem.IsTypeLogEnabled(typeof(IDebugSystem).FullName);
        }

        protected override void OnInit()
        {
            uiSystemTransform = GameObject.Find("UISystem").transform;
            bottomLayer = uiSystemTransform.Find("Bottom");
            middleLayer = uiSystemTransform.Find("Middle");
            topLayer = uiSystemTransform.Find("Top");
        }

        public void SetInteractable(bool isInteractable)
        {
            //Debug.LogError("==> SetInteractable: " + isInteractable);
            this.isInteractable = isInteractable;
            foreach (Transform child in uiSystemTransform)
            {
                var graphicRaycaster = child.GetComponent<GraphicRaycaster>();
                if (graphicRaycaster)
                    graphicRaycaster.enabled = isInteractable;
            }
        }

        public bool IsInteractable()
        {
            return isInteractable;
        }

        public void OpenPanel(PanelType panelType, object param = null)
        {
            //Debug.LogError($"==> OpenPanel: {panelType}");
            GameObject obj = Resources.Load<GameObject>($"Prefabs/Panels/{panelType}");
            var panel = GameObject.Instantiate(obj, bottomLayer);
            panel.GetComponent<UIPanel>().Init(param);
            panelDict.Add(panel, panelType);
        }

        public void OpenMessage(MessageType messageType, MessageInfo messageInfo)
        {
            CloseAllMessages();
            GameObject obj = Resources.Load<GameObject>($"Prefabs/Messages/{messageType}");
            var messageGo = GameObject.Instantiate(obj, topLayer);
            messageGo.GetComponent<UIMessage>().Init(messageInfo);
        }

        public void CloseAllMessages()
        {
            this.SendEvent<ClearAllMessageEvent>();
        }

        public void ClosePanel(GameObject panel, Action onComplete = null)
        {
            panelDict.Remove(panel);
            OnCloseView(panel, onComplete);
        }

        public void ClosePanel(PanelType panelType, Action onComplete = null)
        {
            var panel = panelDict.LastOrDefault(x => x.Value == panelType).Key;
            panelDict.Remove(panel);
            OnCloseView(panel, onComplete);
        }

        public bool HasPopup(PopupType popup)
        {
            return popupDict.ContainsValue(popup);
        }

        public UIPopup OpenPopup(PopupType popupType, object param = null)
        {
            var popup = OpenPopupPure(popupType, param);
            return popup;
        }

        public void ClosePopup(PopupType popupType, Action onComplete = null, bool showCloseAnim = true)
        {
            var popup = popupDict.LastOrDefault(x => x.Value == popupType).Key;
            if (popup != null)
                ClosePopup(popup, onComplete, showCloseAnim);
        }

        public async UniTask OpenPopupAsync(PopupType popupType, object param = null)
        {
            var popup = OpenPopupPure(popupType, param);
            await popup.OnDestroyAsync();
        }

        public async UniTask<bool> OpenPopupAsyncBool(PopupType popupType, object param = null)
        {
            var ret = await OpenPopupAsyncObject(popupType, param);
            return (bool)ret;
        }

        public async UniTask<object> OpenPopupAsyncObject(PopupType popupType, object param = null)
        {
            var popup = OpenPopupPure(popupType, param);
            var ret = await popup.OnDestroyAsyncObject();
            return ret;
        }

        private UIPopup OpenPopupPure(PopupType popupType, object param)
        {
            GameObject obj = Resources.Load<GameObject>($"Prefabs/Popups/{popupType}");
            var popupGo = GameObject.Instantiate(obj, middleLayer);
            popupDict.Add(popupGo, popupType);

            var popup = popupGo.GetComponent<UIPopup>();
            popup.Init(param);
            return popup;
        }

        public void ClosePopup(GameObject popup, Action onComplete = null, bool showCloseAnim = false)
        {
            popup.GetComponent<UIPopup>().OnClosePopupAsync(showCloseAnim, onComplete).Forget();
        }

        public void OnCloseView(GameObject popup, Action onComplete = null)
        {
            popupDict.Remove(popup);
            GameObject.Destroy(popup);
            Resources.UnloadUnusedAssets();
            onComplete?.Invoke();
        }
    }
}

