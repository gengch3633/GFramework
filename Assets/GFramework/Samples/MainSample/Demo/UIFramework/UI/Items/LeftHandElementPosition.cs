using System;
using Framework;
using GameFramework;
using UnityEngine;

namespace GameFramework
{
    public class LeftHandElementPosition : MonoController
    {
        private Vector2 initAnchoredPosition;
        private RectTransform rectTransform;
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            initAnchoredPosition = rectTransform.anchoredPosition;
            UpdatePosition();
        }

        protected override void OnAdListeners()
        {
            base.OnAdListeners();
            settingModel.IsLeftHandOn.RegisterOnValueChanged(v => UpdatePosition())
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void UpdatePosition()
        {
            var newPosition = new Vector3(initAnchoredPosition.x * (settingModel.IsLeftHandOn.Value ? -1 : 1), initAnchoredPosition.y);
            rectTransform.anchoredPosition = newPosition;
        }
    }
}