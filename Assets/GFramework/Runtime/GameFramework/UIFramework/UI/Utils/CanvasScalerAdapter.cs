using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class CanvasScalerAdapter:MonoBehaviour
    {
        private void Awake()
        {
            var canvasScaler = GetComponent<CanvasScaler>();
            var canvas = GetComponent<Canvas>();
            if(canvasScaler != null && canvas != null && canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight)
            {
                var referenceRatio = canvasScaler.referenceResolution.y / canvasScaler.referenceResolution.x;
                var realRatio = Screen.height / Screen.width;
                canvasScaler.matchWidthOrHeight = referenceRatio > realRatio ? 1: 0;
            }
        }
    }
}


