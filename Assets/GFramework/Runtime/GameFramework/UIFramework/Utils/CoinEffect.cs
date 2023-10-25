using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;

namespace GameFramework
{
    public class CoinEffect
    {
        public static async UniTask PlayFlyEffect(Transform srcTransform, Transform  targetTransform, bool initWithSrcTransformStartSize = false)
        {
            var coinCount = 5;
            for (int i = 0; i < coinCount; i++)
            {
                GameObject imageGo = CreateCoin(srcTransform, targetTransform, initWithSrcTransformStartSize);
                Vector3[] _path = GetBezierPoints(srcTransform, targetTransform);
                var targetRectTransform = targetTransform.GetComponent<RectTransform>();
                var targetSize = targetRectTransform.sizeDelta;
                var moveTime = 0.5f;
                DOTween.To(() => targetRectTransform.sizeDelta, (v) => targetRectTransform.sizeDelta = v, targetSize, moveTime).ToUniTask().Forget();
                imageGo.transform.DOPath(_path, moveTime).ToUniTask().Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(0.08f));
            }
        }

        private static Vector3[] GetBezierPoints(Transform srcTransform, Transform targetTransform)
        {
            var startPoint = srcTransform.position;
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

        private static GameObject CreateCoin(Transform srcTransform, Transform targetTransform, bool initWithSrcTransformStartSize = false)
        {
            var startSize = initWithSrcTransformStartSize ? srcTransform.GetComponent<RectTransform>().sizeDelta : targetTransform.GetComponent<RectTransform>().sizeDelta;
            var imageGo = new GameObject("coin", typeof(Image));
            imageGo.AddComponent<CanvasRenderer>();
            imageGo.transform.SetParent(targetTransform);
            imageGo.transform.localScale = Vector3.one;
            imageGo.GetComponent<Image>().sprite = targetTransform.GetComponent<Image>().sprite;
            imageGo.GetComponent<RectTransform>().sizeDelta = startSize;

            imageGo.transform.position = srcTransform.position;
            return imageGo;
        }

        public static Vector3 GetBezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
        {
            return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
        }
    }
}


