using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class BgAdapter : MonoBehaviour
    {
        public EBgType bgType = EBgType.MatchAll;

        private void Start()
        {
            var canvasTransform = transform.parent;
            while(canvasTransform.GetComponent<Canvas>() == null)
                canvasTransform = canvasTransform.parent;
            if(bgType == EBgType.MatchAll)
            {
                var rectTransform = GetComponent<RectTransform>();
                var canvasRectTransform = canvasTransform.GetComponent<RectTransform>();
                rectTransform.sizeDelta = canvasRectTransform.sizeDelta;
            }

            if(bgType == EBgType.Fit)
            {
                var rectTransform = GetComponent<RectTransform>();
                var canvasRectTransform = canvasTransform.GetComponent<RectTransform>();
                var refercenRatio = canvasRectTransform.rect.height / canvasRectTransform.rect.width;
                var bgImageRatio = rectTransform.rect.height / rectTransform.rect.width;

                var widthScale = canvasRectTransform.rect.width / rectTransform.rect.width;
                var heightScale = canvasRectTransform.rect.height / rectTransform.rect.height;
                var usedScale = refercenRatio < bgImageRatio ? widthScale : heightScale;

                rectTransform.sizeDelta = rectTransform.sizeDelta * usedScale;
            }
        }
    }

    public enum EBgType
    {
        MatchAll,
        Fit
    }
}

