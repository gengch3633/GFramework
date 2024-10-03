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
        public ECollectType collectType = ECollectType.GroupCollect;
        public async UniTask CollectItemsAsync(Transform startTransform, BindableProperty<int> bindProperty, int addItemCount)
        {
            gameObject.SetActive(true);
            bindProperty.UnRegisterOnValueChanged(OnUpdateItemCount);
            bindProperty.RegisterOnValueChanged(OnUpdateItemCount)
               .UnRegisterWhenGameObjectDestroyed(gameObject);

            if (collectType == ECollectType.GroupCollect)
                await PlayItemGroupCollectFlyAsync(startTransform, bindProperty, addItemCount);
            else
                await PlayItemCollectFlyAsync(startTransform, bindProperty, addItemCount);
        }

        private async UniTask CollectCoinCountAsync(BindableProperty<int> bindProperty, int addItemCount)
        {
            audioSystem.PlaySound("framework/item_collect");
            var toValue = bindProperty.Value + addItemCount;
            await DOTween.To(() => bindProperty.Value, (v) => bindProperty.Value = v, toValue, 0.5f);
        }

        #region Group Coin Collect
        private float spreadRatio = 1.5f;
        private float minAnimDuration = 0.7f;
        private float maxAnimDuration = 1.0f;
        private Ease flyEase = Ease.InOutBack;
        public float delayTime = 0.5f;
        private async UniTask PlayItemGroupCollectFlyAsync(Transform startTransform, BindableProperty<int> bindProperty, int addItemCount)
        {
            await UniTask.DelayFrame(1);
            var childCount = itemContainerVar.transform.childCount;
            for (int i = 0; i < childCount; i++)
                PlayGroupSingleCoinFlyAsync(startTransform, i).Forget();

            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            await CollectCoinCountAsync(bindProperty, addItemCount);
        }

        private async UniTask PlayGroupSingleCoinFlyAsync(Transform startTransform, int itemIndex)
        {
            var targetTransform = itemIconVar.transform;
            var itemCoin = itemContainerVar.transform.GetChild(itemIndex).GetComponent<Image>();
            itemCoin.gameObject.SetActive(true);
            InitCoin(itemCoin, startTransform, targetTransform);
            var coinWidth = targetTransform.GetComponent<RectTransform>().rect.width;
            var localPositionX = UnityEngine.Random.Range(-spreadRatio, spreadRatio) * coinWidth;
            var localPositionY = UnityEngine.Random.Range(-spreadRatio, spreadRatio) * coinWidth;

            var localPosition = startTransform.localPosition + new Vector3(localPositionX, localPositionY, 0f);
            var worldPosition = startTransform.transform.parent.TransformPoint(localPosition);
            itemCoin.transform.position = worldPosition;

            var duration = UnityEngine.Random.Range(minAnimDuration, maxAnimDuration);
            var targetPosition = itemIconVar.transform.position;
            await itemCoin.transform.DOMove(targetPosition, duration).SetEase(flyEase);
            itemCoin.gameObject.SetActive(false);
        }
        #endregion

        #region Single Collect
        public async UniTask PlayItemCollectFlyAsync(Transform startTransform, BindableProperty<int> bindProperty, int addItemCount)
        {
            var targetTransform = itemIconVar.transform;
            var childCount = itemContainerVar.transform.childCount;
           
            Vector3[] _path = GetBezierPoints(startTransform, targetTransform);
            for (int i = 0; i < childCount; i++)
            {
                PlaySingleCoinFlyAsync(startTransform, i, _path, bindProperty, addItemCount).Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(0.08f));
            }
        }

        private async UniTask PlaySingleCoinFlyAsync(Transform startTransform, int itemIndex, Vector3[] _path, BindableProperty<int> bindProperty, int addItemCount)
        {
            var targetTransform = itemIconVar.transform;
            var moveTime = 0.5f;
            var itemCoin = itemContainerVar.transform.GetChild(itemIndex).GetComponent<Image>();
            GameObject imageGo = itemCoin.gameObject;
            imageGo.gameObject.SetActive(true);
            InitCoin(itemCoin, startTransform, targetTransform);
            await imageGo.transform.DOPath(_path, moveTime);
            if (itemIndex == 0) await CollectCoinCountAsync(bindProperty, addItemCount);
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
        #endregion

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

    public enum ECollectType
    {
        GroupCollect,
        SingleCollect
    }

    public enum ItemType
    {
        Coin,
        Score,
        Diamond
    }
}

