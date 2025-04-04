using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace GameFramework
{
    public partial class UIPanel : UIBasePanel
    {
        protected IGameModel gameModel;
        protected ISettingModel settingModel;
        protected override void MonoAwake()
        {
            base.MonoAwake();
            gameModel = this.GetModel<IGameModel>();
            settingModel = this.GetModel<ISettingModel>();
        }
    }
}

