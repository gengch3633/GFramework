using UnityEngine;

namespace Framework
{
    public class UIAdapter : MonoBehaviour
    {
        public float sizeRangeScale = 0.8f;
        private void Start()
        {
            AdaptVertical();
            AdaptHorizontal();
        }

        private void AdaptVertical()
        {
            var canvasTransform = transform.parent;
            while (canvasTransform.GetComponent<Canvas>() == null)
                canvasTransform = canvasTransform.parent;
            var canvasRectTransform = canvasTransform.GetComponent<RectTransform>();

            var bgContainer = GetComponent<RectTransform>();
            var sizeRangeY = canvasRectTransform.rect.y * sizeRangeScale;
            if (bgContainer.rect.size.y <= sizeRangeY)
                return;

            var adaptScale = sizeRangeY / bgContainer.rect.y;
            bgContainer.localScale = Vector3.one * adaptScale;
        }

        private void AdaptHorizontal()
        {
            var canvasTransform = transform.parent;
            while (canvasTransform.GetComponent<Canvas>() == null)
                canvasTransform = canvasTransform.parent;
            var canvasRectTransform = canvasTransform.GetComponent<RectTransform>();

            var bgContainer = GetComponent<RectTransform>();
            var sizeRangeX = canvasRectTransform.rect.x * 1.0f;
            if (bgContainer.rect.size.x <= sizeRangeX)
                return;


            var adaptScale = sizeRangeX / bgContainer.rect.x;
            if(bgContainer.localScale.x > adaptScale)
                bgContainer.localScale = Vector3.one * adaptScale;
        }
    }
}

