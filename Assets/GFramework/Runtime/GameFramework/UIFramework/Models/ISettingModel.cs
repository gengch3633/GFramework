using System;
using Framework;
using Newtonsoft.Json;
using UnityEngine;

namespace GameFramework
{
    public partial interface ISettingModel : IModel
    {
        BindableProperty<bool> IsSoundOn { get; }
        BindableProperty<bool> IsMusicOn { get; }
        BindableProperty<bool> IsLeftHandOn { get; }
        BindableProperty<bool> IsNotificationOn { get; }
        BindableProperty<bool> IsClickToDiscardOn { get; }
        BindableProperty<bool> IsAutoSortOn { get; }
    }

    public partial class SettingModel : AbstractModel, ISettingModel
    {
        protected override void OnInit()
        {
            var settingRecord = ReadInfoWithReturnNew<SettingModel>();
            CopyBindableClass(this, settingRecord);

            IsSoundOn.RegisterOnValueChanged((v) => SaveInfo(this));
            IsMusicOn.RegisterOnValueChanged((v) => SaveInfo(this));
            IsLeftHandOn.RegisterOnValueChanged((v) => SaveInfo(this));

            IsNotificationOn.RegisterOnValueChanged((v) => SaveInfo(this));
            IsClickToDiscardOn.RegisterOnValueChanged((v) => SaveInfo(this));
            IsAutoSortOn.RegisterOnValueChanged((v) => SaveInfo(this));
        }

        public BindableProperty<bool> IsSoundOn { get; } = new BindableProperty<bool>() { Value = true };
        public BindableProperty<bool> IsMusicOn { get; } = new BindableProperty<bool>() { Value = true };
        public BindableProperty<bool> IsLeftHandOn { get; } = new BindableProperty<bool>();
        public BindableProperty<bool> IsNotificationOn { get; } = new BindableProperty<bool>() { Value = true };
        public BindableProperty<bool> IsClickToDiscardOn { get; } = new BindableProperty<bool>();
        public BindableProperty<bool> IsAutoSortOn { get; } = new BindableProperty<bool>();
    }
}