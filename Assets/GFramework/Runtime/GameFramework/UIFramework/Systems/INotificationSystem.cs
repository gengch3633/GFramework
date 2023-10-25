using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public interface INotificationSystem : ISystem
    {
        
    }

    public class NotificationSystem : AbstractSystem, INotificationSystem, ITypeLog
    {
        private ILocalNotification platformNotification = null;
        private List<string> scheduledNotificationInfo = new List<string>();
        private List<string> lastRespondedNotificationInfo = new List<string>();
        private string nowBackgroundNotificationIdentifier = "";

        protected override void OnInit()
        {
            base.OnInit();
            var isEditor = false;
#if UNITY_EDITOR
            isEditor = true;
#endif

            if(!isEditor) Application.focusChanged += OnApplicationFocus;
#if UNITY_ANDROID
            platformNotification = new LocalNotification_Android();
#elif UNITY_IOS
        platformNotification = new LocalNotificationIOS();
#endif
        }

        public bool IsTypeLogEnabled()
        {
            var debugSystem = GameApp.Interface.GetSystem<IDebugSystem>();
            var ret = debugSystem.IsTypeLogEnabled(typeof(NotificationSystem).FullName);
            return ret;
        }

        private string SendNotification(string title, string bodyText, DateTime dateTime)
        {
            var channelId = Math.Abs(DateTime.Now.ToString("yyMMddHHmmssffffff").GetHashCode()).ToString();
            return platformNotification?.SendNotification(channelId, title, bodyText, dateTime);
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
                BackgroundLogic();
            else
                ComeBackLogic();
        }

        private void BackgroundLogic()
        {
            if (IsTypeLogEnabled()) Debug.LogError($"==> [LocalNotificationManager] BackgroundLogic 时间:{DateTime.Now}");
            var resourceSystem = this.GetSystem<IResourceSystem>();
            var notificationInfos = GameUtils.GetConfigInfos<NotificationInfo>();
            notificationInfos = notificationInfos.FindAll(item => item.notificationType == ENotificationType.Normal);
            var randomId = UnityEngine.Random.Range(0, notificationInfos.Count);
            var notificationInfo = notificationInfos[randomId];

            if (notificationInfo != null)
            {
                var title = string.Format(notificationInfo.notificationTitle, Application.productName);
                var content = string.Format(notificationInfo.notificationContent, Application.productName);
                var identifier = SendNotification(title, content, DateTime.Now.AddMinutes(1));
                EventUtils.LogLocalNofificationScheduledEvent();

                if (IsTypeLogEnabled()) Debug.LogError($"==> [LocalNotificationManager] SendNotification:{identifier}");

                nowBackgroundNotificationIdentifier = identifier;
                if (!string.IsNullOrEmpty(identifier) && !scheduledNotificationInfo.Contains(identifier))
                    scheduledNotificationInfo.Add(identifier);
            }
        }

        private void ComeBackLogic()
        {
            if (IsTypeLogEnabled()) Debug.LogError($"==> [LocalNotificationManager] ComeBackLogic 时间:{DateTime.Now}");
            if (!string.IsNullOrEmpty(nowBackgroundNotificationIdentifier))
            {
                CancelScheduledNotification(nowBackgroundNotificationIdentifier);
                nowBackgroundNotificationIdentifier = "";
            }

            CheckLastNotification();
        }


        //从后台切回来时，检查是否是从推送进入
        private void CheckLastNotification()
        {
            if (platformNotification != null && platformNotification.GetLastRespondedNotification(out string identifier, out string data))
            {
                if (IsTypeLogEnabled()) Debug.LogError($"==> [LocalNotificationManager] CheckLastNotification identifier:{identifier},data:{data}");
                if (!lastRespondedNotificationInfo.Contains(identifier))
                {
                    EventUtils.LogLocalNofificationInvokeAppEvent();
                    platformNotification.CancelNotification(identifier);
                    lastRespondedNotificationInfo.Add(identifier);
                }
            }
        }

        //取消没有显示的(预定中的)推送
        private void CancelScheduledNotification(string identifier)
        {
            if (scheduledNotificationInfo.Contains(identifier))
            {
                platformNotification?.CancelScheduledNotification(identifier);

                scheduledNotificationInfo.Remove(identifier);
                EventUtils.LogLocalNofificationCancelEvent();
            }
        }
    }
    
    public class NotificationInfo
    {
        public int id;
        public ENotificationType notificationType;
        public string notificationTitle;
        public string notificationContent;
    }

    public enum ENotificationType
    {
        Normal,
        TaskFinish,
        TaskNearFinish,
        NeverPlayGame
    }
}

