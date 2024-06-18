using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

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

        public T CreateItem<T>(Transform parent, string suffix = "") where T : Component
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

        public Color ParseColor(string colorString)
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