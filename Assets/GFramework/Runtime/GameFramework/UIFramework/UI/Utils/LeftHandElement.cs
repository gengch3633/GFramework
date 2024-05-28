using System;
using Framework;
using UnityEngine;

namespace GameFramework
{
    public class LeftHandElement : MonoBaseController
    {
        private void Start()
        {
            transform.localScale = new Vector3(settingModel.IsLeftHandOn.Value ? -1 : 1, 1, 1);
            settingModel.IsLeftHandOn.RegisterOnValueChanged(v =>
            {
                transform.localScale = new Vector3(v ? -1 : 1, 1, 1);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
    }
}