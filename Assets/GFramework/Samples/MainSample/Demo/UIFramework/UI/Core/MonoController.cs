using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace GameFramework
{
    public class MonoController : MonoBaseController
    {
        protected ISettingModel settingModel;
        protected override void MonoAwake()
        {
            base.MonoAwake();
            settingModel = this.GetModel<ISettingModel>();
        }
    }
}

