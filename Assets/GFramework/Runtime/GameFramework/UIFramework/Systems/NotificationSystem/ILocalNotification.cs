using System;

namespace GameFramework
{
    public interface ILocalNotification
    {
        string SendNotification(string channelID, string title, string bodyText, DateTime dateTime);
        void CancelScheduledNotification(string identifier);
        void CancelNotification(string identifier);

        bool GetLastRespondedNotification(out string identifier, out string data);
    }
}

