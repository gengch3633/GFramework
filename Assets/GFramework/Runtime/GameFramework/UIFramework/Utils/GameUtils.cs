using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace GameFramework
{
    public class GameUtils
    {
        public static bool IsEditor()
        {
            var isEditor = false;
#if UNITY_EDITOR
            isEditor = true;
#endif

            return isEditor;
        }

        public static void ForEachListWithAction<T>(List<T> cardList, Action<T> action, bool forward = true)
        {
            if (forward)
            {
                for (int i = 0; i < cardList.Count; i++)
                    action.Invoke(cardList[i]);
            }
            else
            {
                for (int i = cardList.Count - 1; i >= 0; i--)
                    action.Invoke(cardList[i]);
            }
        }

        public static void LogElapsedTime(string actionName, Action action, bool isInProfiler = false)
        {
            System.Diagnostics.Stopwatch stopwatch = null;
            if (isInProfiler)
            {
                stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                Profiler.BeginSample(actionName);
            }
               
            action.Invoke();
            if (isInProfiler)
            {
                Profiler.EndSample();
                stopwatch.Stop();
                UnityEngine.Debug.LogError($"==> [LogElapsedTime] [{actionName}]: {stopwatch.ElapsedMilliseconds} ms");
            }
        }

        public static void AddListRange<T>(List<T> toList, List<T> fromList)
        {
            for (int i = 0; i < fromList.Count; i++)
                toList.Add(fromList[i]);
        }

        public static T CreateItem<T>(Transform parent, string suffix = "") where T : Component
        {
            var itemName = typeof(T).Name;
            var itemPrafabPath = suffix == "" ? $"Prefabs/Items/{itemName}" : $"Prefabs/Items/{itemName}_{suffix}";

            var itemPrefab = Resources.Load<GameObject>(itemPrafabPath);
            var tempScoreItem = GameObject.Instantiate(itemPrefab).GetComponent<T>();
            tempScoreItem.transform.SetParent(parent);
            tempScoreItem.transform.localPosition = Vector3.zero;
            tempScoreItem.transform.localScale = Vector3.one;
            return tempScoreItem;
        }

        public static Color ParseColor(string colorString)
        {
            ColorUtility.TryParseHtmlString(colorString, out Color color);
            return color;
        }

        public static List<T> GetConfigInfos<T>(string suffix = "") where T : class, new()
        {
            var itemName = typeof(T).Name;
            var itemPrafabPath = suffix == "" ? $"Data/{itemName}" : $"Data/{itemName}_{suffix}";

            var textAsset = Resources.Load<TextAsset>(itemPrafabPath);
            var textAssetString = textAsset.text;
            var result = JsonConvert.DeserializeObject<List<T>>(textAssetString);
            return result;
        }

        public static T GetConfigInfo<T>(string suffix = "") where T : class, new()
        {
            var itemName = typeof(T).Name;
            var itemPrafabPath = suffix == "" ? $"Data/{itemName}" : $"Data/{itemName}_{suffix}";
            var textAsset = Resources.Load<TextAsset>(itemPrafabPath);
            var textAssetString = textAsset.text;
            var result = JsonConvert.DeserializeObject<T>(textAssetString);
            return result;
        }

        public static async UniTask WaitForFrameDo(int frameCount, Action action)
        {
            await UniTask.DelayFrame(frameCount);
            action.Invoke();
        }
        public static async UniTask WaitForSecondsDo(float seconds, Action action)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds));
            action.Invoke();
        }
    }
}