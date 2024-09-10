using UnityEngine;

namespace GameFramework
{
    public class PopupAdapter : MonoBehaviour
    {
        private void Start()
        {
            var canvasTransform = transform.parent;
            while(canvasTransform.GetComponent<Canvas>() == null)
                canvasTransform = canvasTransform.parent;
            var canvasRectTransform = canvasTransform.GetComponent<RectTransform>();

            var popContainer = transform.Find("Pop");
            if (popContainer == null)
                return;
            
            var bgContainer = popContainer.GetChild(0).GetComponent<RectTransform>();
            var sizeRangeY = canvasRectTransform.rect.height * 0.8f;
            if (bgContainer.rect.height >= sizeRangeY)
            {
                var adaptScale = sizeRangeY / bgContainer.rect.height;
                foreach (Transform t in popContainer)
                    t.localScale = Vector3.one * adaptScale;
            }
        }
    }
}

