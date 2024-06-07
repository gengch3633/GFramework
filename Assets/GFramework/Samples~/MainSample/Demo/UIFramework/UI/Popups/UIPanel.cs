using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace GameFramework
{
    public class UIPanel : UIBasePanel
    {
        protected IGameModel gameModel;
        protected ISettingModel settingModel;
        protected IStatisticsModel statisticsModel;
        protected override void MonoAwake()
        {
            base.MonoAwake();
            gameModel = this.GetModel<IGameModel>();
            settingModel = this.GetModel<ISettingModel>();
            statisticsModel = this.GetModel<IStatisticsModel>();
        }
    }
}

