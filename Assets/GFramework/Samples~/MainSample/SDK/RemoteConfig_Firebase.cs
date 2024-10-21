#if SDK_FIRE_BASE
using Firebase.Extensions;
using Firebase.RemoteConfig;
#endif
using GameFramework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class RemoteConfig_Firebase : ITypeLog
{
    public static bool IsRemoteConfigFetchSuccess = false;
    public static RemoteConfigValue<bool> remoteTestBool = new RemoteConfigValue<bool>("remote_test_bool", true);
    public static RemoteConfigValue<long> remoteTestLong = new RemoteConfigValue<long>("remote_test_long", 100);
    public static RemoteConfigValue<double> remoteTestDouble = new RemoteConfigValue<double>("remote_test_double", 2.0f);
    public static RemoteConfigValue<string> remoteTestString = new RemoteConfigValue<string>("remote_test_string", "remote_test_string");
    private static Dictionary<string, object> defaultRemoteValueDict = new Dictionary<string, object>();

    public RemoteConfig_Firebase()
    {
        InitDefaultRemoteValueDict(remoteTestBool);
        InitDefaultRemoteValueDict(remoteTestLong);
        InitDefaultRemoteValueDict(remoteTestDouble);
        InitDefaultRemoteValueDict(remoteTestString);
        InitConfigValues();
    }

    public void Init(bool isFirebaseInited)
    {
#if SDK_FIRE_BASE
        if (!isFirebaseInited)
            return;

        var configSettings = new ConfigSettings() {  MinimumFetchIntervalInMilliseconds = 4000 };
        FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(configSettings).ContinueWithOnMainThread((Action<Task>)(task =>
        {
            if (task.IsCompleted && task.Exception == null)
                FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaultRemoteValueDict).ContinueWithOnMainThread(task => FetchDataAsync());
        }));
#endif
    }

    private void InitDefaultRemoteValueDict<T>(RemoteConfigValue<T> remoteConfigValue)
    {
#if SDK_FIRE_BASE
        defaultRemoteValueDict.Add(remoteConfigValue.Key, remoteConfigValue.Value);
#endif
    }

    private Task FetchDataAsync()
    {
#if SDK_FIRE_BASE
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
#else
    return null;
#endif
    }

    private void FetchComplete(Task fetchTask)
    {
#if SDK_FIRE_BASE
        ConfigInfo info = FirebaseRemoteConfig.DefaultInstance.Info;
        GameUtils.Log(this, $"fetchTask.IsCanceled: {fetchTask.IsCanceled}, fetchTask.IsFaulted: {fetchTask.IsFaulted}, fetchTask.IsCompleted: {fetchTask.IsCompleted}");
        GameUtils.Log(this, $"info.LastFetchStatus: {info.LastFetchStatus}");

        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                .ContinueWithOnMainThread(task =>
                {
                    IsRemoteConfigFetchSuccess = true;
                    remoteTestBool.SetRemoteValue(this);
                    remoteTestLong.SetRemoteValue(this);
                    remoteTestDouble.SetRemoteValue(this);
                    remoteTestString.SetRemoteValue(this);
                    SetConfigValues();
                });
                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        break;
                    case FetchFailureReason.Throttled:
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                break;
        }
#endif
    }

    public bool IsTypeLogEnabled()
    {
        return GameUtils.IsTypeLogEnabled(this);
    }
}

public class RemoteConfigValue<T>: IRemoteGetValue
{
    public string Key;
    public T Value;

    public RemoteConfigValue(string configKey, T defaultValue)
    {
        this.Key = configKey;
        this.Value = defaultValue;
    }

    public string GetValue()
    {
        return Value.ToString();
    }
    public string GetKey()
    {
        return Key.ToString();
    }


    public void SetRemoteValue(ITypeLog typeLog)
    {
#if SDK_FIRE_BASE
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        if (typeof(T) == typeof(bool))
            Value = (T)(object)remoteConfig.GetValue(this.Key).BooleanValue;
        else if (typeof(T) == typeof(string))
            Value = (T)(object)remoteConfig.GetValue(this.Key).StringValue;
        else if (typeof(T) == typeof(long))
            Value = (T)(object)remoteConfig.GetValue(this.Key).LongValue;
        else if (typeof(T) == typeof(double))
            Value = (T)(object)remoteConfig.GetValue(this.Key).DoubleValue;
        else
        {
            var e = new Exception($"{typeof(T)} TYPE NOT SUPPORTED");
            UnityEngine.Debug.LogError($"==> [NormalMessage] [Init] Exception:\n{e.StackTrace}");
        }
#endif

        GameUtils.Log(typeLog, $"defaultValue: {Value}");
    }
}
public interface IRemoteGetValue
{
    string GetValue();
    string GetKey();
}
