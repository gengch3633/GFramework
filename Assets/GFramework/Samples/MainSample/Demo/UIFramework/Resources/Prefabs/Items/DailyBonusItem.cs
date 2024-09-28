using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class DailyBonusItem : MonoVarController
    {
        public List<TextMeshProUGUI> dayNumTexts;
        public List<TextMeshProUGUI> coinCountTexts;
        public List<Image> coinIcons;
        private Action<int> clickAction;
        private int dayNum = 0;

        public void Init(ESignType eSign, int dayNum, int rewardCount, Action<int> clickAction)
        {
            this.clickAction = clickAction;
            this.dayNum = dayNum;

            dayNumTexts.ForEach(item => item.text = languageSystem.FormatLanguageText("daily_bonus_day_title", (dayNum + 1)));
            coinCountTexts.ForEach(item => item.text = $"+{rewardCount}");
            coinIcons.ForEach(item => { item.sprite = Resources.Load<Sprite>($"Sprites/DailyBonus/daily_bonus_coin_{dayNum+1}"); item.SetNativeSize(); });
            toBeSignedVar.gameObject.SetActive(eSign == ESignType.Sign);
            signedVar.gameObject.SetActive(eSign == ESignType.Signed);
            notSignedVar.gameObject.SetActive(eSign == ESignType.NotSigned);
            dailyBonusItem.interactable = eSign == ESignType.Sign;
        }

        private Button dailyBonusItem;
        private Image toBeSignedVar;
        private Image notSignedVar;
        private Image signedVar;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            dailyBonusItem = transform.Find("").GetComponent<Button>();
            toBeSignedVar = transform.Find("ToBeSigned_Var").GetComponent<Image>();
            notSignedVar = transform.Find("NotSigned_Var").GetComponent<Image>();
            signedVar = transform.Find("Signed_Var").GetComponent<Image>();
            dailyBonusItem.onClick.AddListener(OnDailyBonusItemClick);
        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();
            dailyBonusItem.onClick.RemoveListener(OnDailyBonusItemClick);
        }
        private void OnDailyBonusItemClick() {
            clickAction.Invoke(dayNum);
        }
    }
}

