using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using Cysharp.Threading.Tasks;
using System;

namespace GameFramework
{
    public class CoinContainer : MonoVarController
    {
        public ItemType ItemType;

        public async UniTask DoFlyTokens(Transform startTransform, int coins, bool initWithStartSize = false)
        {
            if (ItemType == ItemType.Coin)
                await DoFlyCoinTokens(startTransform, coins, initWithStartSize);
            else
                await DoFlyStarTokens(startTransform, coins, initWithStartSize);
        }

        public async UniTask DoFlyCoinTokens(Transform startTransform, int coins, bool initWithStartSize = false)
        {
            await CoinEffect.PlayFlyEffect(startTransform.transform, coinEndVar.transform, initWithStartSize);
            audioSystem.PlaySound("collect_coins");
            var toCoin = userModel.Coins.Value + coins;
            await DOTween.To(() => userModel.Coins.Value, (v) => userModel.Coins.Value = v, toCoin, 0.5f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        }

        public async UniTask DoFlyStarTokens(Transform startTransform, int coins, bool initWithStartSize = false)
        {
            await CoinEffect.PlayFlyEffect(startTransform.transform, coinEndVar.transform, initWithStartSize);
            audioSystem.PlaySound("collect_coins");
            var toCoin = userModel.Score.Value + coins;
            await DOTween.To(() => userModel.Score.Value, (v) => userModel.Score.Value = v, toCoin, 0.5f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        }

        protected override void OnAdListeners()
        {
            base.OnAdListeners();
            coinCountTextVar.text = $"{userModel.Coins.Value}";
            userModel.Coins.RegisterOnValueChanged(v => coinCountTextVar.text = $"{userModel.Coins.Value}")
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private Text coinCountTextVar;
        private Image coinContainerVar;
        private Image coinEndVar;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            coinCountTextVar = transform.Find("Bg/result_coin_progress_bg/CoinCountText_Var").GetComponent<Text>();
            coinContainerVar = transform.Find("").GetComponent<Image>();
            coinEndVar = transform.Find("Bg/result_coin_progress_bg/CoinEnd_Var").GetComponent<Image>();

        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();

        }
    }

    public enum ItemType
    {
        Coin,
        Score
    }
}

