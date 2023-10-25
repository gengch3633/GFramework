namespace GameFramework
{
#if UNITY_IOS
using System;
using Unity.Notifications.iOS;
using UnityEngine;

public class LocalNotification_IOS : ILocalNotification
{
    public void CancelNotification(string identifier)
    {
        iOSNotificationCenter.RemoveDeliveredNotification(identifier);
    }

    public void CancelScheduledNotification(string identifier)
    {
        iOSNotificationCenter.RemoveScheduledNotification(identifier);
    }

    public bool GetLastRespondedNotification(out string identifier, out string data)
    {
        var n = iOSNotificationCenter.GetLastRespondedNotification();
        identifier = "";
        data = "";

        if (n != null)
        {
            identifier = n.Identifier;
            data = n.Data;

            var msg = "Last Received Notification : " + n.Identifier + "\n";
            msg += "\n - Notification received: ";
            msg += "\n - .Title: " + n.Title;
            msg += "\n - .Badge: " + n.Badge;
            msg += "\n - .Body: " + n.Body;
            msg += "\n - .CategoryIdentifier: " + n.CategoryIdentifier;
            msg += "\n - .Subtitle: " + n.Subtitle;
            msg += "\n - .Data: " + n.Data;
            Debug.Log(msg);
            return true;
        }
        else
        {
            Debug.Log("No notifications received.");
            return false;
        }
    }

    public string SendNotification(string channelID, string title, string bodyText, DateTime dateTime)
    {
        string identifier = channelID;
        //var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        //{
        //    TimeInterval = new TimeSpan(0, minutes, seconds),
        //    Repeats = false
        //};

        var notificationTrigger = new iOSNotificationCalendarTrigger()
        {
            Year = dateTime.Year,
            Month = dateTime.Month,
            Day = dateTime.Day,
            Hour = dateTime.Hour,
            Minute = dateTime.Minute,
            Second = dateTime.Second,
            Repeats = false,
        };

        var notification = new iOSNotification()
        {
            // You can optionally specify a custom identifier which can later be 
            // used to cancel the notification, if you don't set one, a unique 
            // string will be generated automatically.
            Identifier = channelID,
            Title = title,
            Body = bodyText,
            //Subtitle = "This is a subtitle, something, something important...",
            Subtitle = "",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = notificationTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
        return identifier;
    }

}
#endif
}