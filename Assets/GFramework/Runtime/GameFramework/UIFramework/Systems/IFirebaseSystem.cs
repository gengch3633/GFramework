//#define USE_FIREBASE //gengch

#if USE_FIREBASE
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
#endif
using Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameFramework
{
    public interface IFirebaseSystem : ISystem
    {
        bool IsFirebaseInitialized();
    }

    public class FirebaseSystem : AbstractSystem, IFirebaseSystem, ITypeLog
    {
        private bool firebaseInitialized;
        private const string REMOTE_FISH_MOVE_ROUND = "fish_move_round";
        private Dictionary<string, object> defaulutRemoteConfigInfo = new Dictionary<string, object>(){
            {REMOTE_FISH_MOVE_ROUND, 2}
        };

        protected override void OnInit()
        {
            base.OnInit();
            InitSdk();
        }

        public bool IsTypeLogEnabled()
        {
            var debugSystem = this.GetSystem<IDebugSystem>();
            var ret = debugSystem.IsTypeLogEnabled(typeof(FirebaseSystem).FullName);
            return ret;
        }

        public bool IsFirebaseInitialized()
        {
            return firebaseInitialized;
        }

        private void InitSdk()
        {
#if USE_FIREBASE
            if (!firebaseInitialized)
            {
                if(IsTypeLogEnabled()) Debug.LogError($"==> [FirebaseSystem] InitSdk 1");
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
                {
                    if(IsTypeLogEnabled()) Debug.LogError($"==> [FirebaseSystem] InitSdk 3");
                    var dependencyStatus = task.Result;
                    if (dependencyStatus == DependencyStatus.Available)
                    {
                        firebaseInitialized = true;
                        InitFirebaseRemoteConfig();
                    }
                });
                if(IsTypeLogEnabled()) Debug.LogError($"==> [FirebaseSystem] InitSdk 2");
            }
#else
            firebaseInitialized = true;
#endif

        }

#if USE_FIREBASE
        private void InitFirebaseRemoteConfig()
        {
            try
            {
                if(IsTypeLogEnabled()) Debug.LogError($"==> [FirebaseSystem] InitFirebaseConfig 1");
                FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaulutRemoteConfigInfo).ContinueWithOnMainThread(task => {
                    if(IsTypeLogEnabled()) Debug.LogError($"==> [FirebaseSystem] InitFirebaseConfig 2");
                    FetchDataAsync();
                });

                if(IsTypeLogEnabled()) Debug.LogError($"==> [FirebaseSystem] InitFirebaseConfig 3");
            }
            catch (Exception ex)
            {
                if(IsTypeLogEnabled()) Debug.LogError($"==> [FirebaseSystem] InitFirebaseConfig 4, ex:{ex.Message}");
            }
        }

        private T GetValue<T>(string keyValue)
        {
            T result = default(T);
            if (firebaseInitialized)
            {
                if (typeof(T) == typeof(bool))
                    result = (T)(object)FirebaseRemoteConfig.DefaultInstance.GetValue(keyValue).BooleanValue;
                else if (typeof(T) == typeof(string))
                    result = (T)(object)FirebaseRemoteConfig.DefaultInstance.GetValue(keyValue).StringValue;
                else if (typeof(T) == typeof(long))
                    result = (T)(object)FirebaseRemoteConfig.DefaultInstance.GetValue(keyValue).LongValue;
                else if (typeof(T) == typeof(double))
                    result = (T)(object)FirebaseRemoteConfig.DefaultInstance.GetValue(keyValue).DoubleValue;
                else if (typeof(T) == typeof(int))
                    throw new Exception("INT TYPE NOT SUPPORTED");
            }
            else
            {
                if (defaulutRemoteConfigInfo.ContainsKey(keyValue))
                    result = (T)defaulutRemoteConfigInfo[keyValue];
            }

            return result;
        }

        // Start a fetch request.
        // FetchAsync only fetches new data if the current data is older than the provided
        // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
        // By default the timespan is 12 hours, and for production apps, this is a good
        // number. For this example though, it's set to a timespan of zero, so that
        // changes in the console will always show up immediately.
        private Task FetchDataAsync()
        {
            Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            ConfigInfo info = FirebaseRemoteConfig.DefaultInstance.Info;
            if(IsTypeLogEnabled()) Debug.LogError($"==> [FirebaseSystem] FetchComplete 1, IsCanceled: {fetchTask.IsCanceled}, IsFaulted: {fetchTask.IsFaulted}, IsCompleted: {fetchTask.IsCompleted}");
            if(IsTypeLogEnabled()) Debug.LogError($"==> [FirebaseSystem] FetchComplete 2, info.LastFetchStatus: {info.LastFetchStatus}");
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task => {
                        int fishMoveRound = (int)GetValue<long>(REMOTE_FISH_MOVE_ROUND);
                        if(IsTypeLogEnabled()) Debug.LogError($"==> [FirebaseSystem] FetchComplete 3,  fishMoveRound: {fishMoveRound}");
                    });

                    break;
                case LastFetchStatus.Failure:
                    if(IsTypeLogEnabled()) Debug.LogError($"==> [FirebaseSystem] FetchComplete 4, info.LastFetchFailureReason: {info.LastFetchFailureReason}");
                    break;
            }
        }
        
#endif
    }

}

