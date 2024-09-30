#if SDK_FB
using Facebook.Unity.Editor;
using Facebook.Unity.Settings;
#endif
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameFramework
{
    [InitializeOnLoad]
    public class Define_Facebook
    {
        private static bool logEnabled = true;
        private const string FacebookSettingsFile = "Assets/FacebookSDK/SDK/Resources/FacebookSettings.asset";
        static Define_Facebook()
        {
            logEnabled = false;
            CheckCreateAndSetFacebookSettings();
            logEnabled = true;
        }

        [MenuItem("Tools/3.CheckCreateAndSetFacebookSettings")]
        private static void CheckCreateAndSetFacebookSettings()
        {
            CreateFacebookSettingsAsset();
            SetFacebookSettings();
        }

        private static void CreateFacebookSettingsAsset()
        {
#if SDK_FB

            var settingFileInfo = new FileInfo(FacebookSettingsFile);
            var facebookSettingsResDir = GameUtils.GetAssetPath(settingFileInfo.DirectoryName);
            var settingResDirInfo = new DirectoryInfo(facebookSettingsResDir);
            if (!settingResDirInfo.Exists)
                settingResDirInfo.Create();
            if (!settingFileInfo.Exists)
            {
                var instance = ScriptableObject.CreateInstance<FacebookSettings>();
                AssetDatabase.CreateAsset(instance, FacebookSettingsFile);
                AssetDatabase.SaveAssets();
            }
#endif
        }

        private static void SetFacebookSettings()
        {
#if SDK_FB
            var facebookDefineInfos = GameUtils.GetConfigInfos<FacebookDefineInfo>();
            var facebookSettings = AssetDatabase.LoadAssetAtPath<FacebookSettings>(FacebookSettingsFile);
            FacebookSettings.AppLabels.Clear();
            FacebookSettings.AppIds.Clear();
            FacebookSettings.ClientTokens.Clear();
            foreach (var facebookDefineInfo in facebookDefineInfos)
            {
                FacebookSettings.AppLabels.Add(facebookDefineInfo.facebookAppName);
                FacebookSettings.AppIds.Add(facebookDefineInfo.facebookAppId);
                FacebookSettings.ClientTokens.Add(facebookDefineInfo.facebookTokenId);
            }

            var activeDefineInfo = facebookDefineInfos.Find(item => item.isAndroid);
            var activeIndex = FacebookSettings.AppIds.FindIndex(item => item == activeDefineInfo.facebookAppId);
            FacebookSettings.SelectedAppIndex = activeIndex;

            if (logEnabled)
            {
                Debug.LogError($"==> [FacebookDefine] 1, FacebookSettings.SelectedAppIndex: {FacebookSettings.SelectedAppIndex}");
                Debug.LogError($"==> [FacebookDefine] 2, activeDefineInfo: {JsonConvert.SerializeObject(activeDefineInfo)}");
            }
            
            EditorUtility.SetDirty(facebookSettings);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            ManifestMod.GenerateManifest();
#endif
        }
    }

    public class FacebookDefineInfo
    {
        public int id;
        public bool isAndroid;
        public string facebookAppName;
        public string facebookAppId;
        public string facebookTokenId;
    }
}

