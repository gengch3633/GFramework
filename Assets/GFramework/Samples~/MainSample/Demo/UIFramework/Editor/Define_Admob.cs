using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
#if SDK_ADMOB
using GoogleMobileAds.Editor;
#endif

namespace GameFramework
{
    public class Define_Admob
    {
        private static bool logEnabled = true;
        private const string MobileAdsSettingsFile = "Assets/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset";
        static Define_Admob()
        {
            logEnabled = false;
            SetAdmobSetting();
            logEnabled = true;
        }

        [MenuItem("Tools/SDK参数配置/4.1 设置GoogleMobileAdsSetting可读")]
        private static void AddInternalVisableToGoogleMobileAdsSettings()
        {
            Debug.LogError($"==> [Define_Admob] [AddInternalVisableToGoogleMobileAdsSettings] Start");
            var googleMobileAdsSettingCsFile = "Assets/GoogleMobileAds/Editor/GoogleMobileAdsSettings.cs";
            var fileInfo = new FileInfo(googleMobileAdsSettingCsFile);
            if (!fileInfo.Exists)
                return;
            Debug.LogError($"==> [Define_Admob] [AddInternalVisableToGoogleMobileAdsSettings] 1");
            var fileLines = File.ReadAllLines(googleMobileAdsSettingCsFile).ToList();
            var namespaceLineIndex = fileLines.FindIndex(item => item.Contains("GoogleMobileAds.Editor"));
            var addLine = "[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(\"Assembly-CSharp-Editor\")]";
            var fileText = string.Join("\n", fileLines);
            if (!fileText.Contains(addLine))
            {
                Debug.LogError($"==> [Define_Admob] [AddInternalVisableToGoogleMobileAdsSettings] 21");
                fileLines.Insert(namespaceLineIndex, addLine);
                var newFileText = string.Join("\n", fileLines);
                File.WriteAllText(googleMobileAdsSettingCsFile, newFileText);
            }
            else
                Debug.LogError($"==> [Define_Admob] [AddInternalVisableToGoogleMobileAdsSettings] 22");
            Debug.LogError($"==> [Define_Admob] [AddInternalVisableToGoogleMobileAdsSettings] End");
        }

        [MenuItem("Tools/SDK参数配置/4.2 配置Admob")]
        private static void SetAdmobSetting()
        {
#if SDK_ADMOB
            var adsConfigs = GameUtils.GetConfigInfos<AdsConfig>();
            var googleMobileAdsSettings = AssetDatabase.LoadAssetAtPath<GoogleMobileAdsSettings>(MobileAdsSettingsFile);
            var useTestAds = false;
            var androidAdsConfig = adsConfigs.Find(item => item.buildTarget == BuildTarget.Android && item.useTestAds == useTestAds && item.mediationType == EMediationType.Admob);
            var iosAdsConfig = adsConfigs.Find(item => item.buildTarget == BuildTarget.IOS && item.useTestAds == useTestAds && item.mediationType == EMediationType.Admob);

            googleMobileAdsSettings.GoogleMobileAdsAndroidAppId = androidAdsConfig.appId;
            googleMobileAdsSettings.GoogleMobileAdsIOSAppId = iosAdsConfig.appId;

            googleMobileAdsSettings.EnableKotlinXCoroutinesPackagingOption = true;
            googleMobileAdsSettings.ValidateGradleDependencies = true;
            googleMobileAdsSettings.OptimizeAdLoading = true;
            googleMobileAdsSettings.OptimizeInitialization = true;
            googleMobileAdsSettings.UserTrackingUsageDescription = "Obtain permission for advertising identifiers such as IDFA to provide you with better and safer personalized services and content; after turning it on, you can also go to the system \"Settings - Privacy\" to turn it off at any time.";

            if (logEnabled)
            {
                Debug.LogError($"==> [Define_Admob] 1, androidAdsConfig: {JsonConvert.SerializeObject(androidAdsConfig)}");
                Debug.LogError($"==> [Define_Admob] 2, iosAdsConfig: {JsonConvert.SerializeObject(iosAdsConfig)}");
            }

            EditorUtility.SetDirty(googleMobileAdsSettings);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
#endif
        }
    }
}

