using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;

namespace GameFramework
{
    public class ItemCollectContainer : MonoVarController
    {
        public async UniTask CollectItemsAsync(Transform startTransform, BindableProperty<int> bindProperty, int addItemCount, bool initWithStartSize = false)
        {
            bindProperty.UnRegisterOnValueChanged(OnUpdateItemCount);
            bindProperty.RegisterOnValueChanged(OnUpdateItemCount)
               .UnRegisterWhenGameObjectDestroyed(gameObject);
            await PlayItemCollectFlyAsync(startTransform);
            audioSystem.PlaySound("framework/item_collect");
            var toValue = bindProperty.Value + addItemCount;
            await DOTween.To(() => bindProperty.Value, (v) => bindProperty.Value = v, toValue, 0.5f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        }

        public async UniTask PlayItemCollectFlyAsync(Transform startTransform)
        {
            var targetTransform = itemIconVar.transform;
            var itemIcons = itemContainerVar.GetComponentsInChildren<Image>(true).ToList().FindAll(item => item.gameObject != itemContainerVar.gameObject);
            var itemCount = 5;
            var moveTime = 0.5f;
            Vector3[] _path = GetBezierPoints(startTransform, targetTransform);
            for (int i = 0; i < itemCount; i++)
            {
                var itemCoin = itemIcons[i];
                GameObject imageGo = itemCoin.gameObject;
                imageGo.gameObject.SetActive(true);
                InitCoin(itemIcons[i], startTransform, targetTransform);
                var targetRectTransform = targetTransform.GetComponent<RectTransform>();
                var targetSize = targetRectTransform.sizeDelta;
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

        private void InitCoin(Image iconImage, Transform startTransform, Transform targetTransform)
        {
            var startSize = targetTransform.GetComponent<RectTransform>().sizeDelta;
            iconImage.sprite = targetTransform.GetComponent<Image>().sprite;
            iconImage.GetComponent<RectTransform>().sizeDelta = startSize;
            iconImage.transform.position = startTransform.position;
        }

        private Vector3 GetBezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
        {
            return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
        }

        private void OnUpdateItemCount(int itemCount)
        {
            itemCountTextVar.text = $"{itemCount}";
        }

        private Text itemCountTextVar;
        private Image itemIconVar;
        private Image itemContainerVar;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            itemCountTextVar = transform.Find("ItemCountText_Var").GetComponent<Text>();
            itemIconVar = transform.Find("ItemIcon_Var").GetComponent<Image>();
            itemContainerVar = transform.Find("ItemContainer_Var").GetComponent<Image>();

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

