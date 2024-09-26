using System.Collections.Generic;
#if SDK_FIRE_BASE
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
#endif
using GameFramework;
using System;

public class EventTracker_Firebase : IEventTracker
{
    private bool isFirebaseInited = false;
    private RemoteConfig_Firebase remoteConfig;
    public void Init()
    {
#if SDK_FIRE_BASE
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            isFirebaseInited = task.Result == DependencyStatus.Available;
            remoteConfig = new RemoteConfig_Firebase();
            remoteConfig.Init(isFirebaseInited);
        });
#endif
    }

    public void LogEvent(string eventName, Dictionary<string, object> paramDict)
    {
#if SDK_FIRE_BASE
        if (!isFirebaseInited)
            return;

        var paramList = new List<Parameter>();
        foreach(var item in paramDict)
        {
            if (item.Value.GetType().Name == typeof(Int32).Name)
            {
                var parameter = new Parameter(item.Key, (long)item.Value);
                paramList.Add(parameter);
            }

            if (item.Value.GetType().Name == typeof(Double).Name || item.Value.GetType().Name == typeof(Single).Name)
            {
                var parameter = new Parameter(item.Key, (double)item.Value);
                paramList.Add(parameter);
            }

            if (item.Value.GetType().Name == typeof(String).Name)
            {
                var parameter = new Parameter(item.Key, (string)item.Value);
                paramList.Add(parameter);
            }
        }
        FirebaseAnalytics.LogEvent(eventName, paramList.ToArray());
#endif
    }
}

