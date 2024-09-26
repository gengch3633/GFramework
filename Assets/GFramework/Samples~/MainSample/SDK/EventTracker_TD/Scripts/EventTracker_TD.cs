using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class EventTracker_TD : IEventTracker
{
    public void Init()
    {
#if SDK_TD
        TalkingDataSDK.SetVerboseLogDisable();
        TalkingDataSDK.BackgroundSessionEnabled();
        TalkingDataSDK.InitSDK("EAE937974643492CBC6E6A24AA2E471E", "play.google.com", "your_custom_parameter");
        TalkingDataSDK.StartA();
#endif
    }

    public void LogEvent(string eventName, Dictionary<string, object> paramDict)
    {
#if SDK_TD
        var eventValue = new Dictionary<string, object> { { "key", "value" } };
        TalkingDataSDK.OnEvent(eventName, paramDict, eventValue);
#endif
    }
}
