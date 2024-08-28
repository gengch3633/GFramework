using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace GameFramework
{
    [RequireComponent(typeof(Image))]
    public class BgAdapter : MonoBaseController
    {
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            this.RegisterEvent<OnBgImageChangeEvent>(UpdateImage).
                UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        protected override void MonoStart()
        {
            base.MonoStart();
            UpdateImage();
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateImage();
        }

        private void UpdateImage(OnBgImageChangeEvent evt = null)
        {
            var mainCanvas = GetComponentInParent<Canvas>();
            var canvasTransform = mainCanvas.transform;
            var image = GetComponent<Image>();

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.pivot = Vector2.one * 0.5f;
            rectTransform.anchorMin = Vector2.one * 0.5f;
            rectTransform.anchorMax = Vector2.one * 0.5f;
            var canvasRectTransform = canvasTransform.GetComponent<RectTransform>();
            var imageWidth = image.sprite.rect.width;
            var imageHeight = image.sprite.rect.height;

            var widthScale = canvasRectTransform.rect.width / imageWidth;
            var heightScale = canvasRectTransform.rect.height / imageHeight;
            var usedScale = widthScale > heightScale ? widthScale : heightScale;
            rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight) * usedScale;
        }
    }
}

