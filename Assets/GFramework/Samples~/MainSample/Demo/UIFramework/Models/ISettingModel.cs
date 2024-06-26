using System;
using Framework;
using Newtonsoft.Json;
using UnityEngine;

namespace GameFramework
{
    public partial interface ISettingModel : IModel
    {
        BindableProperty<bool> IsLeftHandOn { get; }
        BindableProperty<bool> IsNotificationOn { get; }
    }

    public partial class SettingModel : AbstractModel, ISettingModel
    {
        protected override void OnInit()
        {
            var settingRecord = ReadInfoWithReturnNew<SettingModel>();
            CopyBindableClass(this, settingRecord, ()=> SaveInfo(this));
        }

        public BindableProperty<bool> IsLeftHandOn { get; } = new BindableProperty<bool>();
        public BindableProperty<bool> IsNotificationOn { get; } = new BindableProperty<bool>() { Value = true };
    }
}