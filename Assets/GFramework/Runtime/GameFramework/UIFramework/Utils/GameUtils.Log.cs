using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System;

namespace GameFramework
{
    public partial class GameUtils
    {
        public static void LogError(ITypeLog typeLog, string message, UnityEngine.Object context)
        {
            if (typeLog.IsTypeLogEnabled())
            {
                string info = GetLogInfo(typeLog, message);
                Debug.LogError(info, context);
            }
        }

        public static void LogError(ITypeLog typeLog, string message = "")
        {
            if (typeLog.IsTypeLogEnabled())
            {
                string info = GetLogInfo(typeLog, message);
                Debug.LogError(info);
            }
        }

        public static void Log(ITypeLog typeLog, string message, UnityEngine.Object context)
        {
            if (typeLog.IsTypeLogEnabled())
            {
                string info = GetLogInfo(typeLog, message);
                Debug.Log(info, context);
            };
        }
        public static void Log(ITypeLog typeLog, string message = "")
        {
            if (typeLog.IsTypeLogEnabled())
            {
                string info = GetLogInfo(typeLog, message);
                Debug.Log(info);
            }
        }
        public static void LogWarning(ITypeLog typeLog, string message, UnityEngine.Object context)
        {
            if (typeLog.IsTypeLogEnabled())
            {
                string info = GetLogInfo(typeLog, message);
                Debug.LogWarning(info, context);
            }
        }
        public static void LogWarning(ITypeLog typeLog, string message = "")
        {
            if (typeLog.IsTypeLogEnabled())
            {
                string info = GetLogInfo(typeLog, message);
                Debug.LogWarning(info);
            }
        }

        private static string GetLogInfo(ITypeLog typeLog, string message = "")
        {
            var stackFrame = new System.Diagnostics.StackTrace(1, true).GetFrame(1);
            var fileName = typeLog.GetType().Name;
            var methodName = stackFrame.GetMethod().Name;
            var useMethodName = !message.Contains("[");
            var info = useMethodName ? $"==> [{fileName}] [{methodName}] {message}" : $"==> [{fileName}] {message}";
            return info;
        }
    }
}