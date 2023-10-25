
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace GameFramework
{
    public static class UnityExtention
    {
        public static T CreateItem<T>(this Transform parent, string suffix = "") where T : Component
        {
            var itemName = typeof(T).Name;
            var itemPrafabPath = suffix == "" ? $"Prefabs/Items/{itemName}" : $"Prefabs/Items/{itemName}_{suffix}";
            var scoreItemPrefab = Resources.Load<GameObject>(itemPrafabPath);
            var tempScoreItem = GameObject.Instantiate(scoreItemPrefab).GetComponent<T>();
            tempScoreItem.transform.SetParent(parent);
            tempScoreItem.transform.localPosition = Vector3.zero;
            tempScoreItem.transform.localScale = Vector3.one;
            return tempScoreItem;
        }


        public static void SetLocalPositionX(this Transform t, float localPositionX)
        {
            var localPosition = t.localPosition;
            localPosition.x = localPositionX;
            t.localPosition = localPosition;
        }

        public static void SetLocalPositionY(this Transform t, float localPositionY)
        {
            var localPosition = t.localPosition;
            localPosition.y = localPositionY;
            t.localPosition = localPosition;
        }

        public static void SetAnchoredPositionX(this Transform ttt, float ancrhordPositionX)
        {
            var t = ttt.GetComponent<RectTransform>();
            var localPosition = t.anchoredPosition;
            localPosition.x = ancrhordPositionX;
            t.anchoredPosition = localPosition;
        }

        public static void SetAnchoredPositionY(this Transform ttt, float ancrhordPositionY)
        {
            var t = ttt.GetComponent<RectTransform>();
            var localPosition = t.localPosition;
            localPosition.y = ancrhordPositionY;
            t.anchoredPosition = localPosition;
        }

        public static void SetToTop(this Transform ttt)
        {

        }

        public static void SetToBottom(this Transform ttt)
        {

        }

        public static async UniTask MoveFromLeft(this RectTransform rectTransform, float time, Ease ease = Ease.Linear)
        {
            var width = rectTransform.sizeDelta.x;
            var toPositionX = rectTransform.anchoredPosition.x;
            var fromPositionX = toPositionX - width;
            rectTransform.SetAnchoredPositionX(fromPositionX);
            await rectTransform.DOAnchorPosX(toPositionX, time).SetEase(ease);
        }

        public static async UniTask MoveToLeft(this RectTransform rectTransform, float time, Ease ease = Ease.Linear)
        {
            var width = rectTransform.sizeDelta.x;
            var initPositionX = rectTransform.anchoredPosition.x;
            var toPositionX = initPositionX - width;
            await rectTransform.DOAnchorPosX(toPositionX, time).SetEase(ease);
        }

        public static async UniTask MoveFromRight(this RectTransform rectTransform, float time, Ease ease = Ease.Linear)
        {
            var width = rectTransform.sizeDelta.x;
            var toPositionX = rectTransform.anchoredPosition.x;
            var fromPositionX = toPositionX + width;
            rectTransform.SetAnchoredPositionX(fromPositionX);
            await rectTransform.DOAnchorPosX(toPositionX, time).SetEase(ease);
        }

        public static async UniTask MoveToRight(this RectTransform rectTransform, float time, Ease ease = Ease.Linear)
        {
            var width = rectTransform.sizeDelta.x;
            var initPositionX = rectTransform.anchoredPosition.x;
            var toPositionX = initPositionX + width;
            await rectTransform.DOAnchorPosX(toPositionX, time).SetEase(ease);
        }
    }
}

