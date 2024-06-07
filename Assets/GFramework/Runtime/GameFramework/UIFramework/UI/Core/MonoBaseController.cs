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
    public class MonoBaseController : MonoBehaviour, IController
    {
        [SerializeField]
        private string tmpFontAssetName;
        [SerializeField]
        private string tmpFontMaterialName;
        protected IUserModel userModel;
        protected IDebugModel debugModel;

        protected ILanguageSystem languageSystem;
        protected IUISystem uiSystem;
        protected IResourceSystem resourceSystem;
        protected IAudioSystem audioSystem;
        protected IRateSystem rateSystem;
        protected IAdsSystem adsSystem;

        protected IFreeCoinSystem freeCoinSystem;
        protected IDailyBonusSystem dailyBonusSystem;
        protected ISpinSystem spinSystem;
        protected IDailyTaskSystem dailyTaskSystem;

        private void Awake()
        {
            MonoAwake();
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
            userModel = this.GetModel<IUserModel>();
            debugModel = this.GetModel<IDebugModel>();

            uiSystem = this.GetSystem<IUISystem>();
            audioSystem = this.GetSystem<IAudioSystem>(); 
            rateSystem = this.GetSystem<IRateSystem>();
            resourceSystem = this.GetSystem<IResourceSystem>();
            languageSystem = this.GetSystem<ILanguageSystem>();
            adsSystem = this.GetSystem<IAdsSystem>();

            freeCoinSystem = this.GetSystem<IFreeCoinSystem>();
            dailyBonusSystem = this.GetSystem<IDailyBonusSystem>();
            spinSystem = this.GetSystem<ISpinSystem>();
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

        [ContextMenu("ChangeBtnTextToTmp")]
        private void ChangeBtnTextToTmp()
        {
            if (string.IsNullOrEmpty(tmpFontAssetName))
                return;
            if (string.IsNullOrEmpty(tmpFontMaterialName))
                return;

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

                TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>(tmpFontAssetName);
                Material material = Resources.Load<Material>(tmpFontMaterialName);
                tmpText.font = fontAsset;
                tmpText.fontSharedMaterial = material;
            });
        }

        [ContextMenu("CreateUIInitMethods")]
        private void CreateUIInitMethods()
        {
            CompCollector.CreateUIInitMethods(gameObject);
        }

        public IArchitecture GetArchitecture()
        {
            return GameApp.Interface;
        }
    }
}


