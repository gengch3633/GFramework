using Cysharp.Threading.Tasks;
using DG.Tweening;
using Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameFramework
{
    public class SpinPopup : UIPopup
    {
        private int radius = 200;
        private List<int> coinCountList = new List<int>() { 10, 500, 20, 300, 30, 250, 50, 200, 100, 150 };
        public override void Init(object param = null)
        {
            base.Init(param);
            InitItems();
            coinContainerVar.gameObject.SetActive(false);
            UpdateBtnState();
        }

        private void UpdateBtnState()
        {
            btnFree.gameObject.SetActive(!spinSystem.IsTodaySpined());
            btnVideo.gameObject.SetActive(spinSystem.IsTodaySpined());
        }

        private void InitItems()
        {
            for (int i = 0; i < coinCountList.Count; i++)
            {
                var coinCount = coinCountList[i];
                var coinCountTextGo = Instantiate(coinCountTextVar.gameObject);
                coinCountTextGo.SetActive(true);
                coinCountTextGo.transform.SetParent(wheelVar.transform);
                coinCountTextGo.transform.localScale = Vector3.one;

                var angle = 360f / coinCountList.Count * i;
                var rad = Mathf.Deg2Rad * angle;
                var localPositin = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad)) * radius;

                coinCountTextGo.transform.localPosition = localPositin;
                coinCountTextGo.transform.localEulerAngles = - new Vector3(0, 0, angle);
                coinCountTextGo.GetComponent<TextMeshProUGUI>().text = $"{coinCount}";
            }
        }

        private void OnSpin()
        {
            btnClose.interactable = false;
            btnVideo.interactable = false;
            btnFree.interactable = false;
            //audioSystem.PlayEffect("turnplate/SpinTurn");
            float spineAngle = -1 * UnityEngine.Random.Range(0, coinCountList.Count) * (360f / coinCountList.Count);

            var anim = DOTween.Sequence();
            var currAngle = -360 + wheelVar.transform.localRotation.eulerAngles.z;
            var stepAngle1 = -360 * 3 + currAngle;
            var stepAngle2 = -360 * 2 + currAngle + spineAngle;

            anim.Append(wheelVar.transform.DORotate(new Vector3(0, 0, stepAngle1), 3f).SetEase(Ease.InSine));
            anim.Append(wheelVar.transform.DORotate(new Vector3(0, 0, stepAngle2), 2f + Mathf.Abs(spineAngle / 360)).SetEase(Ease.OutQuart));
            anim.OnComplete(() => UniTask.Void(OnSpinCompleted));
        }

        private async UniTaskVoid OnSpinCompleted()
        {
            //audioSystem.PlayEffect("turnplate/SpinFinish");
            await UniTask.DelayFrame(1);
            var angle = (wheelVar.transform.localRotation.eulerAngles.z + 360f) % 360f;
            var coinCount = 0;

            for (int i = 0; i < coinCountList.Count; i++)
            {
                var gapAngle = 360f / coinCountList.Count;
                var tempAngle = gapAngle * i;
                if (angle >= (tempAngle - gapAngle / 2) && angle <= (tempAngle + gapAngle / 2))
                    coinCount = coinCountList[i];
            }

            Debug.LogError("==> OnSpinCompleted: " + coinCount);
            coinContainerVar.gameObject.SetActive(true);
            await coinContainerVar.GetComponent<ItemCollectContainer>().CollectItems(wheelSpinVar.transform, userModel.Coins, coinCount);
            coinContainerVar.gameObject.SetActive(false);

            btnClose.interactable = true;
            btnVideo.interactable = true;
            btnFree.interactable = true;
        }

        private Button btnVideo;
        private Button btnFree;
        private Button btnClose;
        private TextMeshProUGUI coinCountTextVar;
        private Image wheelVar;
        private Image wheelSpinVar;
        private Image coinContainerVar;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            btnVideo = transform.Find("Pop/SpinContainer/BtnVideo").GetComponent<Button>();
            btnFree = transform.Find("Pop/SpinContainer/BtnFree").GetComponent<Button>();
            btnClose = transform.Find("Pop/SpinContainer/BtnClose").GetComponent<Button>();
            coinCountTextVar = transform.Find("Pop/SpinContainer/WheelContainer/CoinCountText_Var").GetComponent<TextMeshProUGUI>();
            wheelVar = transform.Find("Pop/SpinContainer/WheelContainer/Wheel_Var").GetComponent<Image>();
            wheelSpinVar = transform.Find("Pop/SpinContainer/WheelContainer/Wheel_Spin_Var").GetComponent<Image>();
            coinContainerVar = transform.Find("CoinContainer_Var").GetComponent<Image>();
            btnVideo.onClick.AddListener(OnBtnVideoClick);
            btnFree.onClick.AddListener(OnBtnFreeClick);
            btnClose.onClick.AddListener(OnBtnCloseClick);
        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();
            btnVideo.onClick.RemoveListener(OnBtnVideoClick);
            btnFree.onClick.RemoveListener(OnBtnFreeClick);
            btnClose.onClick.RemoveListener(OnBtnCloseClick);
        }
        private void OnBtnVideoClick()
        {
            UpdateBtnState();
            adsSystem.CheckShowRewardAdAndRetry("spin_video", (ret) =>
            {
                if (ret) OnSpin();
            });
        }
        private void OnBtnFreeClick()
        {
            spinSystem.SetTodaySpined();
            UpdateBtnState();
            OnSpin();
        }
    }
}