using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    [RequireComponent(typeof(ImageExt))]
    public class ImageAdapter : MonoBehaviour
    {
        public EImageFitType matchType = EImageFitType.Width;

        private void Start()
        {
            UpdateBgSize();
            var imageExt  = GetComponent<ImageExt>();
            imageExt.Init(this);
        }
        public void UpdateBgSize()
        {
            var image = GetComponent<Image>();
            if (image.sprite == null)
                return;

            var imageSize = new Vector2(image.sprite.rect.width, image.sprite.rect.height);
            var rectTransform = transform.GetComponent<RectTransform>();
            var initSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);

            var widthScale = initSize.x / imageSize.x;
            var heightScale = initSize.y / imageSize.y;
            var usedScale = matchType == EImageFitType.Height ? heightScale: widthScale;

            rectTransform.sizeDelta = imageSize * usedScale;
        }
    }
    public enum EImageFitType
    {
        Width,
        Height
    }
}

