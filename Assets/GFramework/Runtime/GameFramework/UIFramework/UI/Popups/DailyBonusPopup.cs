using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace GameFramework
{
    public class DailyBonusPopup : UIPopup
    {
        private List<DailyBonusItem> signItemList;

        public override void Init(object param = null)
        {
            base.Init(param: null);
            InitItems();
            coinContainerVar.gameObject.SetActive(false);
        }

        private void InitItems()
        {
            signItemList = GetComponentsInChildren<DailyBonusItem>().ToList();
            var loginDayNum = dailyBonusSystem.GetDailyBonusDayNum();
            for (int dayNum = 0; dayNum < signItemList.Count; dayNum++)
            {
                var signItem = signItemList[dayNum];
                var eSign = dayNum < loginDayNum ? ESignType.Signed : dayNum == loginDayNum ? (dailyBonusSystem.IsDailyBonusClaimed(dayNum)? ESignType.Signed: ESignType.Sign) : ESignType.NotSigned;
                signItem.Init(eSign, dayNum, dailyBonusSystem.GetDailyBonusRewardCount(dayNum), OnClickDayItem);
            }
        }

        private async void OnClickDayItem(int dayNum)
        {
            btnClose.interactable = false;
            var loginDayNum = dailyBonusSystem.GetDailyBonusDayNum();
            if (!dailyBonusSystem.IsDailyBonusClaimed(loginDayNum))
            {
                dailyBonusSystem.ClaimDailyBonus(loginDayNum);
                var rewardCount = dailyBonusSystem.GetDailyBonusRewardCount(loginDayNum);
                var signItem = signItemList[loginDayNum % signItemList.Count];
                coinContainerVar.gameObject.SetActive(true);
                InitItems();
                await coinContainerVar.GetComponent<CoinContainer>().DoFlyCoinTokens(signItem.coinIcons[0].transform, rewardCount);
                coinContainerVar.gameObject.SetActive(false);
            }
            btnClose.interactable = true;

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            OnBtnCloseClick();
        }

        private Button btnClose;
        private Image coinContainerVar;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            btnClose = transform.Find("Pop/BtnClose").GetComponent<Button>();
            coinContainerVar = transform.Find("CoinContainer_Var").GetComponent<Image>();
            btnClose.onClick.AddListener(OnBtnCloseClick);
        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();
            btnClose.onClick.RemoveListener(OnBtnCloseClick);
        }
    }
}