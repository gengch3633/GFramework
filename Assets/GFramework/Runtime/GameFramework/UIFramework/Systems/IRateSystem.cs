using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;
using Newtonsoft.Json;

namespace GameFramework
{
    public interface IRateSystem : ISystem
    {
        void IncreaseLevel();

        bool IsGoodTimeForRate();

        void OnRate(int starCount, Action gotoEmailAciton = null, Action gotoGooglePlayPageAction = null);

        void OnNoRate();

        void OpenGooglePlayPage();
        void OpenContactEmailPage();
        void OpenPrivacyPage();

        BindableProperty<bool> IsRated { get; }
        BindableProperty<DateTime> LastActionTime { get; }
        BindableProperty<int> LastActionLevel { get; }
        BindableProperty<int> CompletedLevels { get; }
    }

    public class RateSystem : AbstractSystem, IRateSystem, ITypeLog
    {
        private string publishUrl = $"https://play.google.com/store/apps/details?id={Application.identifier}";
        private string subject = $"Suggestion about {Application.productName}";
        public BindableProperty<bool> IsRated { get; } = new BindableProperty<bool>() { Value = false };
        public BindableProperty<DateTime> LastActionTime { get; } = new BindableProperty<DateTime>() { Value = DateTime.MinValue };
        public BindableProperty<int> LastActionLevel { get; } = new BindableProperty<int>() { Value = 0 };
        public BindableProperty<int> CompletedLevels { get; } = new BindableProperty<int>() { Value = 0 };

        protected override void OnInit()
        {
            var rateRecord = new RateSystem();
            CopyBindableClass(this, rateRecord);

            GameUtils.Log(this, "publishUrl: " + publishUrl);
            GameUtils.Log(this, "supportEmail: " + SDKConst.sdkConfig.supportEmail);
            GameUtils.Log(this, "subject: " + subject);

            GameUtils.Log(this, "RateSystem 1: " + GetInfo(this));
        }

        public bool IsTypeLogEnabled()
        {
            return GameUtils.IsTypeLogEnabled(this);
        }

        public bool IsGoodTimeForRate()
        {
            //已经投票5个星星了
            if (this.IsRated.Value)
                return false;

            int passLevels = this.CompletedLevels.Value;
            //第一次弹出
            if (passLevels - this.LastActionLevel.Value >= 6)
                return true;
            // 每6关，或弹屏超过24小时
            if ((this.LastActionTime.Value > DateTime.MinValue) && (DateTime.Now - this.LastActionTime.Value).TotalHours >= 24)
                return true;

            return false;
        }

        public void OnRate(int starCount, Action gotoEmailAciton = null, Action gotoGooglePlayPageAction = null)
        {
            this.LastActionTime.Value = DateTime.Now;
            this.LastActionLevel.Value= this.CompletedLevels.Value;
            if (starCount >= 5)
            {
                this.IsRated.Value = true;
                OpenGooglePlayPage();
                gotoGooglePlayPageAction?.Invoke();
            }
            else
            {
                OpenContactEmailPage();
                gotoEmailAciton?.Invoke();
            }
            GameUtils.Log(this, "RateSystem 2: " + GetInfo(this));
        }

        public void OnNoRate()
        {
            this.LastActionTime.Value = DateTime.Now;
            this.LastActionLevel.Value = this.CompletedLevels.Value;
            GameUtils.Log(this, "RateSystem 3: " + GetInfo(this));
        }

        public void OpenGooglePlayPage()
        {
            Application.OpenURL(this.publishUrl);
        }

        public void OpenPrivacyPage()
        {
            Application.OpenURL(SDKConst.sdkConfig.privacyPage);
        }

        public void OpenContactEmailPage()
        {
            string email = SDKConst.sdkConfig.supportEmail;
            string subject = MyEscapeURL(this.subject);
            var suggestionString = string.Format("My problem/suggestion is:", Application.productName);
            var gapString = "*************************";
            var dontDeleteString = "Please don't delete the important info below!";
            var applicationInfo = $"Platform: {Application.platform}\nVersion: {Application.version}\nPackageName: {Application.identifier}";
            var deviveInfo = $"DeviceModel: {SystemInfo.deviceModel}\nOSVersion: {Environment.OSVersion}\nScreenSize: {Screen.currentResolution}\nLanguage: {Application.systemLanguage}";
            var infoString = $"{gapString}\n{dontDeleteString}\n{applicationInfo}\n{deviveInfo}";
            var oriBody = $"{suggestionString}\n\n\n\n{infoString}";
            string body = MyEscapeURL(oriBody);
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);

            GameUtils.Log(this, "body: " + body);
        }

        private string MyEscapeURL(string url)
        {
            return WWW.EscapeURL(url).Replace("+", "%20");
        }

        public void IncreaseLevel()
        {
            this.CompletedLevels.Value += 1;
            GameUtils.Log(this, "RateSystem 4: " + GetInfo(this));
        }
    }
}

