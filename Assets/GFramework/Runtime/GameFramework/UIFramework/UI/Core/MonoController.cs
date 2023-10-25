using Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class MonoController : MonoBehaviour, IController
    {
        protected IGameModel gameModel;
        protected ILanguageSystem languageSystem;
        protected IUISystem uiSystem;
        protected IResourceSystem resourceSystem;
        protected IAudioSystem audioSystem;
        protected IRateSystem rateSystem;
        protected IFreeCoinSystem freeCoinSystem;
        protected IDebugSystem debugSystem;
        protected IAdsSystem adsSystem;
        protected ISdkSystem sdkSystem;
        protected IFirebaseSystem firebaseSystem;
        protected IDailyBonusSystem dailyBonusSystem;
        protected ISpinSystem spinSystem;

        protected IUserModel userModel;
        protected ISettingModel settingModel;
        protected ITutorialModel tutorialModel;
        protected IStatisticsModel statisticsModel;

        protected IDailyTaskSystem dailyTaskSystem;

        private void Awake()
        {
            MonoAwake();
            MonoAwakePartial();
            OnAddUIListeners();
            OnAdListeners();
            OnInitVars();
        }

        private void Start()
        {
            MonoStart();
        }

        protected virtual void OnInitVars()
        {

        }

        protected virtual void MonoAwake()
        {
            gameModel = this.GetModel<IGameModel>();

            languageSystem = this.GetSystem<ILanguageSystem>();
            uiSystem = this.GetSystem<IUISystem>();
            resourceSystem = this.GetSystem<IResourceSystem>();
            audioSystem = this.GetSystem<IAudioSystem>();
            rateSystem = this.GetSystem<IRateSystem>();
            freeCoinSystem = this.GetSystem<IFreeCoinSystem>();
            debugSystem = this.GetSystem<IDebugSystem>();
            adsSystem = this.GetSystem<IAdsSystem>();

            firebaseSystem = this.GetSystem<IFirebaseSystem>();
            sdkSystem = this.GetSystem<ISdkSystem>();
            dailyBonusSystem = this.GetSystem<IDailyBonusSystem>();
            spinSystem = this.GetSystem<ISpinSystem>();

            userModel = this.GetModel<IUserModel>();
            settingModel = this.GetModel<ISettingModel>();
            tutorialModel = this.GetModel<ITutorialModel>();
            statisticsModel = this.GetModel<IStatisticsModel>();

            dailyTaskSystem = this.GetSystem<IDailyTaskSystem>();
        }

        protected virtual void MonoAwakePartial()
        {
            
        }

        protected virtual void MonoStart()
        {

        }

        protected virtual void OnAdListeners()
        {
            this.RegisterEvent<LanguageChangedEvent>(evt => OnLanguageChanged())
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            OnLanguageChanged();
        }

        protected virtual void OnAddUIListeners()
        {
            
        }
        protected virtual void OnLanguageChanged()
        {

        }

        protected virtual void OnRemoveUIListeners()
        {

        }

        protected virtual void OnDestroy()
        {
            OnRemoveUIListeners();
        }

        [ContextMenu("AddLocalizedText")]
        private void AddLocalizedText()
        {
            var textList = GetComponentsInChildren<Text>(true).ToList();
            var tmpTextList = GetComponentsInChildren<TextMeshProUGUI>(true).ToList();
            float widthScale = 1.6f;
            float heightScale = 1.2f;
            textList.ForEach(item =>
            {
                var localizedText = item.gameObject.AddComponent<LocalizedText>();
                item.horizontalOverflow = HorizontalWrapMode.Wrap;
                item.verticalOverflow = VerticalWrapMode.Truncate;
                item.resizeTextMaxSize = item.fontSize;
                item.resizeTextForBestFit = true;
                if (item.alignment != TextAnchor.MiddleCenter) return;
                var itemTrans = item.GetComponent<RectTransform>();
                //if (item.alignment == TextAnchor.MiddleLeft)
                //{
                //    var v = new Vector2(0, 0.5f);
                //    itemTrans.anchorMin = v;
                //    itemTrans.anchorMax = v;
                //    itemTrans.pivot = v;
                //}

                var maxHeight = item.fontSize * heightScale > itemTrans.rect.size.y ? item.fontSize * heightScale : itemTrans.rect.size.y;
                itemTrans.sizeDelta = new Vector2(item.preferredWidth * widthScale, maxHeight);
            });

            tmpTextList.ForEach(item =>
            {
                var localizedText = item.gameObject.AddComponent<LocalizedText>();
                item.enableWordWrapping = true;
                item.enableAutoSizing = true;
                item.fontSizeMax = item.fontSize;
                if (item.verticalAlignment != VerticalAlignmentOptions.Middle) return;

                var itemTrans = item.GetComponent<RectTransform>();
                var maxHeight = item.fontSize * heightScale > itemTrans.rect.size.y ? item.fontSize * heightScale : itemTrans.rect.size.y;
                itemTrans.sizeDelta = new Vector2(item.preferredWidth * widthScale, maxHeight);
            });

        }

        [ContextMenu("GetLocalizedText")]
        private void GetLocalizedText()
        {
            var textList = GetComponentsInChildren<LocalizedText>(true).ToList();
            textList = textList.FindAll(item => !string.IsNullOrEmpty(item.key) && item != null);
            Dictionary<string, string> itemDict = new Dictionary<string, string>();
            Func<LocalizedText, string> getText = (item) =>
            {
                var textComponent = item.GetComponent<Text>();
                var tmpTextComponent = item.GetComponent<TextMeshProUGUI>();
                if (textComponent != null) return textComponent.text.Replace("\r", "");
                if (tmpTextComponent != null) return tmpTextComponent.text.Replace("\r", "");
                return "None";

            };
            textList.ForEach(item => itemDict[item.key] = getText(item));

            Debug.LogError("==> itemDict:\n" + JsonConvert.SerializeObject(itemDict));
        }

        [ContextMenu("ChangeBtnTextToTmp")]
        private void ChangeBtnTextToTmp()
        {
            var btns = GetComponentsInChildren<Button>().ToList();
            var tmpBtns = btns.FindAll(item => item.GetComponentInChildren<Text>() != null);
            var tmpTexts = tmpBtns.ConvertAll(item => item.GetComponentInChildren<Text>());
            tmpTexts.ForEach(item => {
                var itemGo = new GameObject($"{item.name}_TMP", typeof(TextMeshProUGUI));
                itemGo.transform.SetParent(item.transform.parent);
                itemGo.transform.localScale = Vector3.one;
                itemGo.transform.position = item.transform.position;

                var tmpText = itemGo.GetComponent<TextMeshProUGUI>();
                tmpText.text = item.text;
                tmpText.fontSize = item.fontSize;
                tmpText.fontStyle = FontStyles.Bold;
                tmpText.horizontalAlignment = HorizontalAlignmentOptions.Center;
                tmpText.verticalAlignment = VerticalAlignmentOptions.Middle;
                tmpText.GetComponent<RectTransform>().sizeDelta = item.GetComponent<RectTransform>().rect.size;

                TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>("Roboto-Bold SDF");
                Material material = Resources.Load<Material>("Roboto-Bold SDF Material_Btn_Green");
                tmpText.font = fontAsset;
                tmpText.fontSharedMaterial = material;
            });
        }

        [ContextMenu("AddButtonAnim")]
        private void AddButtonAnim()
        {
            var btns = GetComponentsInChildren<Button>().ToList();
            btns.ForEach(item => {
                var btnGo = item.gameObject;
                DestroyImmediate(btnGo.GetComponent<Button>());

                btnGo.AddComponent<ClickBtnAni_Small>();
            });
            CreateBtnListenerMethods();
        }


        [ContextMenu("AddButtonClickSound")]
        private void AddButtonClickSound()
        {
            var btns = GetComponentsInChildren<Button>(true).ToList();
            btns.ForEach(item => {
                var btnGo = item.gameObject;
                if (item.GetComponent<ClickSound>() == null)
                    item.gameObject.AddComponent<ClickSound>();
            });
        }

        [ContextMenu("CreateBtnListenerMethods")]
        private void CreateBtnListenerMethods()
        {
            CompCollector.CreateBtnListenerMethods(gameObject);
        }


        public IArchitecture GetArchitecture()
        {
            return GameApp.Interface;
        }
    }

}


