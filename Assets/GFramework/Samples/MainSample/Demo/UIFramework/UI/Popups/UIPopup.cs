using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace GameFramework
{
    public partial class UIPopup : UIBasePopup
    {
        protected IGameModel gameModel;
        protected ISettingModel settingModel;
        protected override void MonoAwake()
        {
            base.MonoAwake();
            gameModel = this.GetModel<IGameModel>();
            settingModel = this.GetModel<ISettingModel>();
        }

        protected string GetParamString(object param)
        {
            var openFrom = param == null ? "no_param" : (string)param;
            return openFrom;
        }
    }
}

