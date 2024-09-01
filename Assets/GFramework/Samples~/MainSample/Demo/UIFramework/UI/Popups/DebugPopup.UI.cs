
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public partial class DebugPopup
    {
        protected override void OnInitVars()
        {
            base.OnInitVars();
            var languageNames = System.Enum.GetNames(typeof(LanguageType)).ToList();
            var languageIndex = languageNames.IndexOf(languageSystem.GetLanguageType().ToString());
            languageDropdown.ClearOptions();
            languageDropdown.AddOptions(languageNames);
            languageDropdown.value = languageIndex;
        }

        private Button btnClose;
        private Button btnChangeLevel;
        private Button btnClearData;
        private Button btnAddCoin;
        private Button btnClearCoin;
        private Button btnSaveGameData;
        private Button btnRecoverGameData;
        private Toggle toggleDebugFeature;
        private Toggle toggleTypeLog;
        private Toggle toggleNormal;
        private Toggle item;
        private TMP_InputField levelIdInput;
        private ScrollRect debugFeatureContainerScrollView;
        private ScrollRect typeLogContainerScrollView;
        private ScrollRect normalContainerScrollView;
        private ScrollRect template;
        private Dropdown languageDropdown;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            btnClose = transform.Find("Pop/Container/TopContainer/BtnClose").GetComponent<Button>();
            btnChangeLevel = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnChangeLevel").GetComponent<Button>();
            btnClearData = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnClearData").GetComponent<Button>();
            btnAddCoin = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnAddCoin").GetComponent<Button>();
            btnClearCoin = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnClearCoin").GetComponent<Button>();
            btnSaveGameData = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnSaveGameData").GetComponent<Button>();
            btnRecoverGameData = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnRecoverGameData").GetComponent<Button>();
            toggleDebugFeature = transform.Find("Pop/Container/MiddleContainer/ToggleDebugFeature").GetComponent<Toggle>();
            toggleTypeLog = transform.Find("Pop/Container/MiddleContainer/ToggleTypeLog").GetComponent<Toggle>();
            toggleNormal = transform.Find("Pop/Container/MiddleContainer/ToggleNormal").GetComponent<Toggle>();
            item = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/LanguageDropdown/Template/Viewport/Content/Item").GetComponent<Toggle>();
            levelIdInput = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/LevelIdInput").GetComponent<TMP_InputField>();
            debugFeatureContainerScrollView = transform.Find("Pop/Container/BottomContainer/DebugFeatureContainerScrollView").GetComponent<ScrollRect>();
            typeLogContainerScrollView = transform.Find("Pop/Container/BottomContainer/TypeLogContainerScrollView").GetComponent<ScrollRect>();
            normalContainerScrollView = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView").GetComponent<ScrollRect>();
            template = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/LanguageDropdown/Template").GetComponent<ScrollRect>();
            languageDropdown = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/LanguageDropdown").GetComponent<Dropdown>();
            btnClose.onClick.AddListener(OnBtnCloseClick);
            btnChangeLevel.onClick.AddListener(OnBtnChangeLevelClick);
            btnClearData.onClick.AddListener(OnBtnClearDataClick);
            btnAddCoin.onClick.AddListener(OnBtnAddCoinClick);
            btnClearCoin.onClick.AddListener(OnBtnClearCoinClick);
            btnSaveGameData.onClick.AddListener(OnBtnSaveGameDataClick);
            btnRecoverGameData.onClick.AddListener(OnBtnRecoverGameDataClick);
            toggleDebugFeature.onValueChanged.AddListener(OnToggleDebugFeatureChanged);
            toggleTypeLog.onValueChanged.AddListener(OnToggleTypeLogChanged);
            toggleNormal.onValueChanged.AddListener(OnToggleNormalChanged);
            item.onValueChanged.AddListener(OnItemChanged);
            levelIdInput.onEndEdit.AddListener(OnLevelIdInputChanged);
            languageDropdown.onValueChanged.AddListener(OnLanguageDropdownChanged);
        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();
            btnClose.onClick.RemoveListener(OnBtnCloseClick);
            btnChangeLevel.onClick.RemoveListener(OnBtnChangeLevelClick);
            btnClearData.onClick.RemoveListener(OnBtnClearDataClick);
            btnAddCoin.onClick.RemoveListener(OnBtnAddCoinClick);
            btnClearCoin.onClick.RemoveListener(OnBtnClearCoinClick);
            btnSaveGameData.onClick.RemoveListener(OnBtnSaveGameDataClick);
            btnRecoverGameData.onClick.RemoveListener(OnBtnRecoverGameDataClick);
            toggleDebugFeature.onValueChanged.RemoveListener(OnToggleDebugFeatureChanged);
            toggleTypeLog.onValueChanged.RemoveListener(OnToggleTypeLogChanged);
            toggleNormal.onValueChanged.RemoveListener(OnToggleNormalChanged);
            item.onValueChanged.RemoveListener(OnItemChanged);
            levelIdInput.onEndEdit.RemoveListener(OnLevelIdInputChanged);
            languageDropdown.onValueChanged.RemoveListener(OnLanguageDropdownChanged);
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

            Debug.LogError($"==> OnLanguageDropdownChanged: {languageType}");
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
            userModel.Coins.Value += 10000;
            userModel.Diamonds.Value += 10000;
        }
        private void OnBtnClearCoinClick() {

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


