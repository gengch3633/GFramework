
using TMPro;
using UnityEngine.UI;

namespace GameFramework
{
    public partial class DebugPopup : UIPopup
    {
        private Button btnClose;
        private Button btnAddCoin;
        private Button btnClearCoin;
        private Button btnChangeLevel;
        private Button btnSaveGameData;
        private Button btnRecoverGameData;
        private Toggle toggleDebugFeature;
        private Toggle toggleTypeLog;
        private Toggle toggleNormal;
        private TMP_InputField levelIdInput;
        private Text winRateTextVar;
        private ScrollRect debugFeatureContainerScrollView;
        private ScrollRect typeLogContainerScrollView;
        private ScrollRect normalContainerScrollView;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            btnClose = transform.Find("Pop/Container/TopContainer/BtnClose").GetComponent<Button>();
            btnAddCoin = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnAddCoin").GetComponent<Button>();
            btnClearCoin = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnClearCoin").GetComponent<Button>();
            btnChangeLevel = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnChangeLevel").GetComponent<Button>();
            btnSaveGameData = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnSaveGameData").GetComponent<Button>();
            btnRecoverGameData = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/BtnRecoverGameData").GetComponent<Button>();
            toggleDebugFeature = transform.Find("Pop/Container/MiddleContainer/ToggleDebugFeature").GetComponent<Toggle>();
            toggleTypeLog = transform.Find("Pop/Container/MiddleContainer/ToggleTypeLog").GetComponent<Toggle>();
            toggleNormal = transform.Find("Pop/Container/MiddleContainer/ToggleNormal").GetComponent<Toggle>();
            levelIdInput = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/LevelIdInput").GetComponent<TMP_InputField>();
            winRateTextVar = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView/Viewport/Content/ActionItem/WinRateText_Var").GetComponent<Text>();
            debugFeatureContainerScrollView = transform.Find("Pop/Container/BottomContainer/DebugFeatureContainerScrollView").GetComponent<ScrollRect>();
            typeLogContainerScrollView = transform.Find("Pop/Container/BottomContainer/TypeLogContainerScrollView").GetComponent<ScrollRect>();
            normalContainerScrollView = transform.Find("Pop/Container/BottomContainer/NormalContainerScrollView").GetComponent<ScrollRect>();
            btnClose.onClick.AddListener(OnBtnCloseClick);
            btnAddCoin.onClick.AddListener(OnBtnAddCoinClick);
            btnClearCoin.onClick.AddListener(OnBtnClearCoinClick);
            btnChangeLevel.onClick.AddListener(OnBtnChangeLevelClick);
            btnSaveGameData.onClick.AddListener(OnBtnSaveGameDataClick);
            btnRecoverGameData.onClick.AddListener(OnBtnRecoverGameDataClick);
            toggleDebugFeature.onValueChanged.AddListener(OnToggleDebugFeatureChanged);
            toggleTypeLog.onValueChanged.AddListener(OnToggleTypeLogChanged);
            toggleNormal.onValueChanged.AddListener(OnToggleNormalChanged);
            levelIdInput.onEndEdit.AddListener(OnLevelIdInputChanged);
        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();
            btnClose.onClick.RemoveListener(OnBtnCloseClick);
            btnAddCoin.onClick.RemoveListener(OnBtnAddCoinClick);
            btnClearCoin.onClick.RemoveListener(OnBtnClearCoinClick);
            btnChangeLevel.onClick.RemoveListener(OnBtnChangeLevelClick);
            btnSaveGameData.onClick.RemoveListener(OnBtnSaveGameDataClick);
            btnRecoverGameData.onClick.RemoveListener(OnBtnRecoverGameDataClick);
            toggleDebugFeature.onValueChanged.RemoveListener(OnToggleDebugFeatureChanged);
            toggleTypeLog.onValueChanged.RemoveListener(OnToggleTypeLogChanged);
            toggleNormal.onValueChanged.RemoveListener(OnToggleNormalChanged);
            levelIdInput.onEndEdit.RemoveListener(OnLevelIdInputChanged);
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
        }

        private void OnBtnClearCoinClick() {

            userModel.Coins.Value -= userModel.Coins.Value;
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


