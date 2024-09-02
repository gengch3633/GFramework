using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using Cysharp.Threading.Tasks;
using System;

namespace GameFramework
{
    public class ItemCollectContainer : MonoVarController
    {
        public async UniTask CollectItems(Transform startTransform, BindableProperty<int> bindProperty, int addItemCount, bool initWithStartSize = false)
        {
            await PlayItemCollectFlyAsync(startTransform, initWithStartSize);
            audioSystem.PlaySound("framework/item_collect");
            var toValue = bindProperty.Value + addItemCount;
            await DOTween.To(() => bindProperty.Value, (v) => bindProperty.Value = v, toValue, 0.5f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        }

        public async UniTask PlayItemCollectFlyAsync(Transform startTransform, bool initWithSrcTransformStartSize = false)
        {
            var targetTransform = coinEndVar.transform;
            var itemCount = 5;
            for (int i = 0; i < itemCount; i++)
            {
                GameObject imageGo = CreateCoin(startTransform, targetTransform, initWithSrcTransformStartSize);
                Vector3[] _path = GetBezierPoints(startTransform, targetTransform);
                var targetRectTransform = targetTransform.GetComponent<RectTransform>();
                var targetSize = targetRectTransform.sizeDelta;
                var moveTime = 0.5f;
                DOTween.To(() => targetRectTransform.sizeDelta, (v) => targetRectTransform.sizeDelta = v, targetSize, moveTime).ToUniTask().Forget();
                imageGo.transform.DOPath(_path, moveTime).ToUniTask().Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(0.08f));
            }
        }

        private Vector3[] GetBezierPoints(Transform startTransform, Transform targetTransform)
        {
            var startPoint = startTransform.position;
            var endPoint = targetTransform.position;
            var bezierControlPoint = (startPoint + endPoint) * 0.5f + 0.5f * (Vector3.up * (endPoint.y - startPoint.y));

            var resolution = 1000;

            var _path = new Vector3[resolution];
            for (int i = 0; i < resolution; i++)
            {
                var t = (i + 1) / (float)resolution;
                _path[i] = GetBezierPoint(t, startPoint, bezierControlPoint, endPoint);
            }

            return _path;
        }

        private GameObject CreateCoin(Transform startTransform, Transform targetTransform, bool initWithSrcTransformStartSize = false)
        {
            var startSize = initWithSrcTransformStartSize ? startTransform.GetComponent<RectTransform>().sizeDelta : targetTransform.GetComponent<RectTransform>().sizeDelta;
            var imageGo = new GameObject("Item", typeof(Image));
            imageGo.AddComponent<CanvasRenderer>();
            imageGo.transform.SetParent(targetTransform);
            imageGo.transform.localScale = Vector3.one;
            imageGo.GetComponent<Image>().sprite = targetTransform.GetComponent<Image>().sprite;
            imageGo.GetComponent<RectTransform>().sizeDelta = startSize;

            imageGo.transform.position = startTransform.position;
            return imageGo;
        }

        private Vector3 GetBezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
        {
            return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
        }

        protected override void OnAdListeners()
        {
            base.OnAdListeners();
            coinCountTextVar.text = $"{userModel.Coins.Value}";
            userModel.Coins.RegisterOnValueChanged(v => coinCountTextVar.text = $"{userModel.Coins.Value}")
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private Text coinCountTextVar;
        private Image coinEndVar;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            coinCountTextVar = transform.Find("CoinCountText_Var").GetComponent<Text>();
            coinEndVar = transform.Find("CoinEnd_Var").GetComponent<Image>();

        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();

        }
    }

    public enum ItemType
    {
        Coin,
        Score,
        Diamond
    }
}

