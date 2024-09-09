using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class PurchaseLoadingPopup : UIBasePopup
    {
        private float rotateAngle = 240;
        private float currentAngle = 0;
        private void Update()
        {
            currentAngle += rotateAngle * Time.deltaTime;
            iconLoadingVar.transform.localEulerAngles = new Vector3(0, 0, currentAngle);
        }
        private Image iconLoadingVar;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            iconLoadingVar = transform.Find("bg/IconLoading_Var").GetComponent<Image>();

        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();

        }
    }
}

