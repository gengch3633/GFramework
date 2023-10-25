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
        private string publishUrl;
        private string subject;
        private string body;


        private RateRecord rateRecord;
        protected override void OnInit()
        {
            publishUrl = string.Format("https://play.google.com/store/apps/details?id={0}", Application.identifier);
            subject = string.Format("Suggestion about {0}", Application.productName);
            body = string.Format("Hello {0} Team, \n", Application.productName);

            if(IsTypeLogEnabled()) Debug.LogError("==> [RateSystem] publishUrl: " + publishUrl);
            if(IsTypeLogEnabled()) Debug.LogError("==> [RateSystem] supportEmail: " + SDKConst.supportEmail);
            if(IsTypeLogEnabled()) Debug.LogError("==> [RateSystem] subject: " + subject);
            if(IsTypeLogEnabled()) Debug.LogError("==> [RateSystem] body: " + body);

            rateRecord = new RateRecord();
            if (PlayerPrefs.HasKey(Key))
                rateRecord = JsonConvert.DeserializeObject<RateRecord>(PlayerPrefs.GetString(Key));
        }

        public bool IsTypeLogEnabled()
        {
            var debugSystem = this.GetSystem<IDebugSystem>();
            var ret = debugSystem.IsTypeLogEnabled(typeof(RateSystem).FullName);
            return ret;
        }

        public bool IsGoodTimeForRate()
        {
            //已经投票5个星星了
            if (rateRecord.isRated)
                return false;

            int passLevels = rateRecord.completedLevels;
            //第一次弹出
            if (passLevels - rateRecord.lastActionLevel >= 6)
                return true;
            // 每6关，或弹屏超过24小时
            if ((rateRecord.lastActionTime > DateTime.MinValue) && (DateTime.Now - rateRecord.lastActionTime).TotalHours >= 24)
                return true;

            return false;
        }

        public void OnRate(int starCount)
        {
            rateRecord.lastActionTime = DateTime.Now;
            rateRecord.lastActionLevel = rateRecord.completedLevels;
            if (starCount >= 5)
            {
                rateRecord.isRated = true;
                OpenGooglePlayPage();
            }
            else
                OpenContactEmailPage();
            SaveInfo();
        }

        public void OnNoRate()
        {
            rateRecord.lastActionTime = DateTime.Now;
            rateRecord.lastActionLevel = rateRecord.completedLevels;
            SaveInfo();
        }

        private void SaveInfo()
        {
            PlayerPrefs.SetString(Key, JsonConvert.SerializeObject(rateRecord));
            //DebugUtility.LogError("==> rateRecord: " + JsonConvert.SerializeObject(rateRecord));
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
            rateRecord.completedLevels += 1;
            SaveInfo();
        }

        
    }

    public class RateRecord
    {
        public DateTime lastActionTime = DateTime.MinValue;
        public bool isRated = false;
        public int lastActionLevel = 0;
        public int completedLevels = 0;
    }
}

