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
        void SetInteractable(bool isInteractable);
        bool IsInteractable();
        
        void OnCloseView(GameObject popup, Action onComplete = null);

        #region UIMessage
        void CloseAllMessages();
        void OpenMessage<T>(MessageInfo messageInfo) where T : UIBaseMessage;
        #endregion

        #region UIPanel
        void OpenPanel<T>(object param = null) where T : UIBasePanel;
        void ClosePanel(GameObject panel, Action onComplete = null);
        void ClosePanel<T>(Action onComplete = null) where T : UIBasePanel;
        #endregion

        #region UIPopup
        bool HasPopup<T>() where T : UIBasePopup;
        T OpenPopup<T>(object param = null) where T: UIBasePopup;
        UniTask OpenPopupAsync<T>(object param = null) where T : UIBasePopup;
        UniTask<T2> OpenPopupAsync<T1, T2>(object param = null) where T1 : UIBasePopup;
        void ClosePopup<T>(Action onComplete = null, bool showCloseAnim = true) where T : UIBasePopup;
        void ClosePopup(GameObject popup, Action onComplete, bool showCloseAnim);
        #endregion
    }
    public class UISystem: AbstractSystem, IUISystem, ITypeLog
    {
        private Transform uiSystem;
        private Transform bottomLayer;
        private Transform middleLayer;
        private Transform topLayer;

        private Dictionary<GameObject, string> popupDict = new Dictionary<GameObject, string>();
        private Dictionary<GameObject, string> panelDict = new Dictionary<GameObject, string>();
        private bool isInteractable = true;

        public bool IsTypeLogEnabled()
        {
            return GameUtils.IsTypeLogEnabled(this);
        }

        protected override void OnInit()
        {
            uiSystem = GameObject.Find("UISystem").transform;
            bottomLayer = uiSystem.Find("Bottom");
            middleLayer = uiSystem.Find("Middle");
            topLayer = uiSystem.Find("Top");
        }

        public void SetInteractable(bool isInteractable)
        {
            this.isInteractable = isInteractable;
            foreach (Transform child in uiSystem)
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

        public void OnCloseView(GameObject popup, Action onComplete = null)
        {
            popupDict.Remove(popup);
            GameObject.Destroy(popup);
            Resources.UnloadUnusedAssets();
            onComplete?.Invoke();
        }


        #region UIMessage
        public void OpenMessage<T>(MessageInfo messageInfo) where T:UIBaseMessage
        {
            CloseAllMessages();
            var messageViewName = typeof(T).Name;
            GameObject obj = Resources.Load<GameObject>($"Prefabs/Messages/{messageViewName}");
            GameUtils.LogElapsedTime($"==> [OpenMessage]: [{messageViewName}]", () =>
            {
                var messageGo = GameObject.Instantiate(obj, topLayer);
                messageGo.GetComponent<UIBaseMessage>().Init(messageInfo);
            }, IsTypeLogEnabled());
        }
        public void CloseAllMessages()
        {
            this.SendEvent<ClearAllMessageEvent>();
        }
        #endregion

        #region UIPanel
        public void OpenPanel<T>(object param = null) where T : UIBasePanel
        {
            var panelName = typeof(T).Name;
            GameUtils.LogElapsedTime($"==> [OpenPanel]: [{panelName}]", () =>
            {
                GameObject obj = Resources.Load<GameObject>($"Prefabs/Panels/{panelName}");
                var panel = GameObject.Instantiate(obj, bottomLayer);
                panel.GetComponent<UIBasePanel>().Init(param);
                panelDict.Add(panel, panelName);
            }, IsTypeLogEnabled());
        }
        public void ClosePanel(GameObject panel, Action onComplete = null)
        {
            panelDict.Remove(panel);
            OnCloseView(panel, onComplete);
        }
        public void ClosePanel<T>(Action onComplete = null) where T : UIBasePanel
        {
            var panelName = typeof(T).Name;
            var panel = panelDict.LastOrDefault(x => x.Value == panelName).Key;
            panelDict.Remove(panel);
            OnCloseView(panel, onComplete);
        }
        #endregion

        #region UIPopup
        public bool HasPopup<T>() where T: UIBasePopup
        {
            var popupName = typeof(T).Name;
            return popupDict.ContainsValue(popupName);
        }
        public void ClosePopup(GameObject popup, Action onComplete = null, bool showCloseAnim = false)
        {
            popup.GetComponent<UIBasePopup>().OnClosePopupAsync(showCloseAnim, onComplete).Forget();
        }
        public void ClosePopup<T>(Action onComplete = null, bool showCloseAnim = true) where T: UIBasePopup
        {
            var popupName = typeof(T).Name;
            var popup = popupDict.LastOrDefault(x => x.Value == popupName).Key;
            if (popup != null)
                ClosePopup(popup, onComplete, showCloseAnim);
        }
        public T OpenPopup<T>(object param = null) where T : UIBasePopup
        {
            var popUpName = typeof(T).Name;
            return (T)OpenPopup(popUpName, param);
        }
        public async UniTask OpenPopupAsync<T>(object param = null) where T : UIBasePopup
        {
            var popUpName = typeof(T).Name;
            var popup = OpenPopup(popUpName, param);
            await popup.OnDestroyAsyncObject();
        }
        public async UniTask<T2> OpenPopupAsync<T1, T2>(object param = null) where T1 : UIBasePopup
        {
            var popUpName = typeof(T1).Name;
            var popup = OpenPopup(popUpName, param);
            var ret = (T2)(await popup.OnDestroyAsyncObject());
            return ret;
        }
        private UIBasePopup OpenPopup(string popupName, object param)
        {
            GameObject obj = Resources.Load<GameObject>($"Prefabs/Popups/{popupName}");
            UIBasePopup popup = null;
            GameUtils.LogElapsedTime($"==> [OpenPopup]: [{popupName}]", () =>
            {
                var popupGo = GameObject.Instantiate(obj, middleLayer);
                popupDict.Add(popupGo, popupName);

                popup = popupGo.GetComponent<UIBasePopup>();
                popup.Init(param);
            }, IsTypeLogEnabled());
            return popup;
        }
        #endregion
    }
}

