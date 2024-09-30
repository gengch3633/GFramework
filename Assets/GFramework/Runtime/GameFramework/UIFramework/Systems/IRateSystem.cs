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

        void OnRate(int starCount);

        void OnNoRate();

        void OpenGooglePlayPage();
        void OpenContactEmailPage();
        void OpenPrivacyPage();
    }

    public class RateSystem : AbstractSystem, IRateSystem, ITypeLog
    {
        private static readonly string Key = "RATE_SYSTEM";
        private string publishUrl = string.Format("https://play.google.com/store/apps/details?id={0}", Application.identifier);
        private string subject = string.Format("Suggestion about {0}", Application.productName);
        private string body = string.Format("Hello {0} Team, \n", Application.productName);

        public DateTime lastActionTime = DateTime.MinValue;
        public bool isRated = false;
        public int lastActionLevel = 0;
        public int completedLevels = 0;

        protected override void OnInit()
        {
            var rateRecord = new RateSystem();
            CopyBindableClass(this, rateRecord);

            GameUtils.Log(this, "publishUrl: " + publishUrl);
            GameUtils.Log(this, "supportEmail: " + SDKConst.sdkConfig.supportEmail);
            GameUtils.Log(this, "subject: " + subject);
            GameUtils.Log(this, "body: " + body);
        }

        public bool IsTypeLogEnabled()
        {
            return GameUtils.IsTypeLogEnabled(this);
        }

        public bool IsGoodTimeForRate()
        {
            //已经投票5个星星了
            if (this.isRated)
                return false;

            int passLevels = this.completedLevels;
            //第一次弹出
            if (passLevels - this.lastActionLevel >= 6)
                return true;
            // 每6关，或弹屏超过24小时
            if ((this.lastActionTime > DateTime.MinValue) && (DateTime.Now - this.lastActionTime).TotalHours >= 24)
                return true;

            return false;
        }

        public void OnRate(int starCount)
        {
            this.lastActionTime = DateTime.Now;
            this.lastActionLevel = this.completedLevels;
            if (starCount >= 5)
            {
                this.isRated = true;
                OpenGooglePlayPage();
            }
            else
                OpenContactEmailPage();
            SaveInfo(this);
        }

        public void OnNoRate()
        {
            this.lastActionTime = DateTime.Now;
            this.lastActionLevel = this.completedLevels;
            SaveInfo(this);
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
            string body = MyEscapeURL(this.body);
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }

        private string MyEscapeURL(string url)
        {
            return WWW.EscapeURL(url).Replace("+", "%20");
        }

        public void IncreaseLevel()
        {
            this.completedLevels += 1;
            SaveInfo(this);
        }
    }
}

