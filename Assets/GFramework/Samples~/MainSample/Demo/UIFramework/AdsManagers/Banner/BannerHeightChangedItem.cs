using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using Framework;

namespace Solitaire
{
    public class BannerHeightChangedItem : MonoController
    {
        public EBannerHeightChangedType changeType;
        private RectTransform selfRectTrans;
        private Vector2 offsetMin;
        protected override void OnInitVars()
        {
            base.OnInitVars();
            selfRectTrans = GetComponent<RectTransform>();
            offsetMin = selfRectTrans.offsetMin;
            BannerHeightChanged(new BannerHeightChangedEvent(userModel.BannerHeight.Value));
        }
        protected override void OnAdListeners()
        {
            base.OnAdListeners();
            this.RegisterEvent<BannerHeightChangedEvent>(BannerHeightChanged)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void BannerHeightChanged(BannerHeightChangedEvent evt)
        {
            var bannerHeight = evt.bannerHeight;
            if(changeType == EBannerHeightChangedType.OffsetMin)
            {
                var newOffsetMin = offsetMin + new Vector2(0, bannerHeight);
                selfRectTrans.offsetMin = newOffsetMin;
            }

            if (changeType == EBannerHeightChangedType.PositionY)
            {
                var canvas = GetComponentInParent<Canvas>();
                var canvasRectTrans = canvas.GetComponent<RectTransform>();
                var canvasHeight = canvasRectTrans.rect.height;
                var selfHeight = selfRectTrans.rect.height;
                var inCanvasPositionY = canvasRectTrans.InverseTransformPoint(selfRectTrans.position).y;

                var canvasBottomY = -canvasHeight / 2;
                var selfBottomYInCanvas = inCanvasPositionY - selfHeight / 2;
                var twoRectOffsetBottomY = selfBottomYInCanvas - canvasBottomY;
                if (twoRectOffsetBottomY > bannerHeight)
                    return;

                var offsetY = Mathf.Abs(twoRectOffsetBottomY - bannerHeight);
                selfRectTrans.SetLocalPositionY(selfRectTrans.localPosition.y + offsetY);

                AdaptHeight(bannerHeight);
            }
        }

        private void AdaptHeight(float bannerHeight)
        {
            var canvas = GetComponentInParent<Canvas>();
            var canvasRectTrans = canvas.GetComponent<RectTransform>();
            var canvasHeight = canvasRectTrans.rect.height;
            var selfHeight = selfRectTrans.rect.height;
            var inCanvasPositionY = canvasRectTrans.InverseTransformPoint(selfRectTrans.position).y;

            var canvasBottomY = -canvasHeight / 2;
            var selfBottomYInCanvas = inCanvasPositionY - selfHeight / 2;
            var topPostionY = selfBottomYInCanvas + selfHeight;
            var topCanvasPositionY = canvasHeight / 2;
            if (topPostionY > topCanvasPositionY)
            {
                var overflowHeight = topPostionY - topCanvasPositionY;
                var overflowOffsetY = -overflowHeight / 2;
                selfRectTrans.SetLocalPositionY(selfRectTrans.localPosition.y + overflowOffsetY);
                var scale = (canvasHeight - bannerHeight) / selfHeight;
                selfRectTrans.localScale = Vector3.one * scale;
            }
        }
    }

    public enum EBannerHeightChangedType
    {
        PositionY,
        OffsetMin
    }
}

