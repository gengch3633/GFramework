
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public partial class DebugPopup: ITypeLog
    {
        private string password = "asdf";
        public bool IsTypeLogEnabled()
        {
            return GameUtils.IsTypeLogEnabled(this);
        }
        protected override void OnInitVars()
        {
            base.OnInitVars();
            var languageNames = System.Enum.GetNames(typeof(LanguageType)).ToList();
            var languageIndex = languageNames.IndexOf(languageSystem.GetLanguageType().ToString());
            languageDropdown.ClearOptions();
            languageDropdown.AddOptions(languageNames);
            languageDropdown.value = languageIndex;
            var isDebugSign = debugModel.IsDebugFeatureEnabled<Debug_DebugSign>();
            passWordContainerVar.gameObject.SetActive(!isDebugSign);
        }

        private ItemCollectContainer coinCollectContainerVar;
        private ItemCollectContainer diamondCollectContainerVar;
        private Button btnClose;
        private Button btnClearData;
        private Button btnAddCoin;
        private Button btnClearCoin;
        private Button btnOpenAdContainer;
        private Button btnSaveGameData;
        private Button btnRecoverGameData;
        private Button btnSignIn;
        private Toggle toggleDebugFeature;
        private Toggle toggleTypeLog;
        private Toggle toggleNormal;
        private Toggle item;
        private InputField passWordInputField;
        private Image containerVar;
        private Image passWordContainerVar;
        private ScrollRect debugFeatureContainerScrollView;
        private ScrollRect typeLogContainerScrollView;
        private ScrollRect normalContainerScrollView;
        private ScrollRect template;
        private Dropdown languageDropdown;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            coinCollectContainerVar = transform.Find("Pop/Container/CoinCollectContainer_Var").GetComponent<ItemCollectContainer>();
            diamondCollectContainerVar = transform.Find("Pop/Container/DiamondCollectContainer_Var").GetComponent<ItemCollectContainer>();
            btnClose = transform.Find("Pop/Container/TopContainer/BtnClose").GetComponent<Button>();
            btnClearData = transform.Find("Pop/Container/Container_Var/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnClearData").GetComponent<Button>();
            btnAddCoin = transform.Find("Pop/Container/Container_Var/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnAddCoin").GetComponent<Button>();
            btnClearCoin = transform.Find("Pop/Container/Container_Var/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnClearCoin").GetComponent<Button>();
            btnOpenAdContainer = transform.Find("Pop/Container/Container_Var/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnOpenAdContainer").GetComponent<Button>();
            btnSaveGameData = transform.Find("Pop/Container/Container_Var/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnSaveGameData").GetComponent<Button>();
            btnRecoverGameData = transform.Find("Pop/Container/Container_Var/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnRecoverGameData").GetComponent<Button>();
            btnSignIn = transform.Find("Pop/Container/Container_Var/PassWordContainer_Var/BtnSignIn").GetComponent<Button>();
            toggleDebugFeature = transform.Find("Pop/Container/Container_Var/MiddleContainer/ToggleDebugFeature").GetComponent<Toggle>();
            toggleTypeLog = transform.Find("Pop/Container/Container_Var/MiddleContainer/ToggleTypeLog").GetComponent<Toggle>();
            toggleNormal = transform.Find("Pop/Container/Container_Var/MiddleContainer/ToggleNormal").GetComponent<Toggle>();
            item = transform.Find("Pop/Container/Container_Var/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/LanguageDropdown/Template/Viewport/Content/Item").GetComponent<Toggle>();
            passWordInputField = transform.Find("Pop/Container/Container_Var/PassWordContainer_Var/PassWordInputField").GetComponent<InputField>();
            containerVar = transform.Find("Pop/Container/Container_Var").GetComponent<Image>();
            passWordContainerVar = transform.Find("Pop/Container/Container_Var/PassWordContainer_Var").GetComponent<Image>();
            debugFeatureContainerScrollView = transform.Find("Pop/Container/Container_Var/BottomContainer/DebugFeatureContainerScrollView").GetComponent<ScrollRect>();
            typeLogContainerScrollView = transform.Find("Pop/Container/Container_Var/BottomContainer/TypeLogContainerScrollView").GetComponent<ScrollRect>();
            normalContainerScrollView = transform.Find("Pop/Container/Container_Var/BottomContainer/NormalContainerScrollView").GetComponent<ScrollRect>();
            template = transform.Find("Pop/Container/Container_Var/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/LanguageDropdown/Template").GetComponent<ScrollRect>();
            languageDropdown = transform.Find("Pop/Container/Container_Var/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/LanguageDropdown").GetComponent<Dropdown>();
            btnClose.onClick.AddListener(OnBtnCloseClick);
            btnClearData.onClick.AddListener(OnBtnClearDataClick);
            btnAddCoin.onClick.AddListener(OnBtnAddCoinClick);
            btnClearCoin.onClick.AddListener(OnBtnClearCoinClick);
            btnOpenAdContainer.onClick.AddListener(OnBtnOpenAdContainerClick);
            btnSaveGameData.onClick.AddListener(OnBtnSaveGameDataClick);
            btnRecoverGameData.onClick.AddListener(OnBtnRecoverGameDataClick);
            btnSignIn.onClick.AddListener(OnBtnSignInClick);
            toggleDebugFeature.onValueChanged.AddListener(OnToggleDebugFeatureChanged);
            toggleTypeLog.onValueChanged.AddListener(OnToggleTypeLogChanged);
            toggleNormal.onValueChanged.AddListener(OnToggleNormalChanged);
            item.onValueChanged.AddListener(OnItemChanged);
            passWordInputField.onEndEdit.AddListener(OnPassWordInputFieldChanged);
            languageDropdown.onValueChanged.AddListener(OnLanguageDropdownChanged);
        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();
            btnClose.onClick.RemoveListener(OnBtnCloseClick);
            btnClearData.onClick.RemoveListener(OnBtnClearDataClick);
            btnAddCoin.onClick.RemoveListener(OnBtnAddCoinClick);
            btnClearCoin.onClick.RemoveListener(OnBtnClearCoinClick);
            btnOpenAdContainer.onClick.RemoveListener(OnBtnOpenAdContainerClick);
            btnSaveGameData.onClick.RemoveListener(OnBtnSaveGameDataClick);
            btnRecoverGameData.onClick.RemoveListener(OnBtnRecoverGameDataClick);
            btnSignIn.onClick.RemoveListener(OnBtnSignInClick);
            toggleDebugFeature.onValueChanged.RemoveListener(OnToggleDebugFeatureChanged);
            toggleTypeLog.onValueChanged.RemoveListener(OnToggleTypeLogChanged);
            toggleNormal.onValueChanged.RemoveListener(OnToggleNormalChanged);
            item.onValueChanged.RemoveListener(OnItemChanged);
            passWordInputField.onEndEdit.RemoveListener(OnPassWordInputFieldChanged);
            languageDropdown.onValueChanged.RemoveListener(OnLanguageDropdownChanged);
        }
        
        private void OnPassWordInputFieldChanged(string value)
        {

        }
        private void OnBtnSignInClick()
        {
            var isRightSignIn = passWordInputField.text == password;
            passWordContainerVar.gameObject.SetActive(!isRightSignIn);
            if (isRightSignIn)
                debugModel.SetDebugFeatureEnabled(typeof(Debug_DebugSign).FullName, true);
        }

        private void OnBtnOpenAdContainerClick()
        {
            var adsItem = GameUtils.CreateItem<AdsItem>(containerVar.transform);
            var adsItemRectTransform = adsItem.GetComponent<RectTransform>();
            adsItemRectTransform.offsetMin = Vector2.zero;
            adsItemRectTransform.offsetMax = Vector2.zero;
        }


        private void OnBtnClearDataClick()
        {
        }
      
        private void OnItemChanged(bool value)
        {

        }

        private void OnLanguageDropdownChanged(int value)
        {
            var languageNames = System.Enum.GetNames(typeof(LanguageType)).ToList();
            var languageName = languageNames[value];
            System.Enum.TryParse(languageName, out LanguageType languageType);

            GameUtils.Log(this, $"languageType: {languageType}");
            languageSystem.SetLanguageType(languageType);
        }
        private void OnBtnSaveGameDataClick() {
            debugModel.CopyGameData();
        }
        private void OnBtnRecoverGameDataClick() {
            debugModel.RecoverGameDataFromFile();
        }
        private void OnBtnAddCoinClick() 
        {
            var addItemCount = 1000;
            coinCollectContainerVar.CollectItemsAsync(btnAddCoin.transform, userModel.Coins, addItemCount).Forget();
            diamondCollectContainerVar.CollectItemsAsync(btnAddCoin.transform, userModel.Diamonds, addItemCount).Forget();
        }
        private void OnBtnClearCoinClick() 
        {
            userModel.Coins.Value -= userModel.Coins.Value;
            userModel.Diamonds.Value -= userModel.Diamonds.Value;
        }
        private void OnBtnChangeLevelClick() {
            ChangeDifficultyLevelClick();
        }
        private void OnToggleNormalChanged(bool value)
        {
            normalContainerScrollView.gameObject.SetActive(value);
            debugFeatureContainerScrollView.gameObject.SetActive(!value);
            typeLogContainerScrollView.gameObject.SetActive(!value);
        }
        private void OnToggleDebugFeatureChanged(bool value)
        {
            normalContainerScrollView.gameObject.SetActive(!value);
            debugFeatureContainerScrollView.gameObject.SetActive(value);
            typeLogContainerScrollView.gameObject.SetActive(!value);
        }
        private void OnToggleTypeLogChanged(bool value)
        {
            normalContainerScrollView.gameObject.SetActive(!value);
            debugFeatureContainerScrollView.gameObject.SetActive(!value);
            typeLogContainerScrollView.gameObject.SetActive(value);
        }
        private void OnLevelIdInputChanged(string value)
        {
            TrySetDiffLv(value);
        }
    }
}


