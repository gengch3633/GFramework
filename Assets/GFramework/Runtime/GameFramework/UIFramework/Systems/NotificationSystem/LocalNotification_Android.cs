#if UNITY_ANDROID
using System;
using Unity.Notifications.Android;
using UnityEngine;

namespace GameFramework
{
    public class LocalNotification_Android : ILocalNotification
    {
        public string SendNotification(string channelID, string title, string bodyText, DateTime dateTime)
        {
            AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel()
            {
                Id = channelID,
                Name = $"Android_{channelID}",
                Importance = Importance.High,
                Description = "Generic notifications"
            };
            AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

            AndroidNotification notification = new AndroidNotification()
            {
                Title = title,
                Text = bodyText,
                //FireTime = System.DateTime.Now
                FireTime = dateTime
            };
            return AndroidNotificationCenter.SendNotification(notification, channelID).ToString();
        }

        public void CancelScheduledNotification(string identifierStr)
        {
            if (int.TryParse(identifierStr, out int identifier))
            {
                if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Scheduled)
                {
                    AndroidNotificationCenter.CancelScheduledNotification(identifier);
                }
            }
        }

        public bool GetLastRespondedNotification(out string identifier, out string data)
        {
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
            identifier = "";
            data = "";

            if (notificationIntentData != null)
            {
                identifier = notificationIntentData.Id.ToString();
                //var channel = notificationIntentData.Channel;
                data = notificationIntentData.Notification.IntentData;

                Debug.Log($"GetLastRespondedNotification Id:{notificationIntentData.Id},Channel:{notificationIntentData.Channel},data:{data}");

                return true;
            }
            else
            {
                return false;
            }

        }

        public void CancelNotification(string identifierStr)
        {
            if (int.TryParse(identifierStr, out int identifier))
            {
                if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Delivered)
                {
                    AndroidNotificationCenter.CancelNotification(identifier);
                }
            }
        }
    }
#endif
}
