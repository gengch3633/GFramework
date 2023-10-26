using UnityEditor;
using Facebook.Unity.Settings;
using GoogleMobileAds.Editor;
using System.IO;
using UnityEngine;
using GameFramework;
using Newtonsoft.Json;

[InitializeOnLoad]
public class KeystoreSetter
{
    static KeystoreSetter()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "Assets/GFramework/Editor/royale_gin_rummy.keystore";
        PlayerSettings.Android.keystorePass = "123456";
        PlayerSettings.Android.keyaliasName = "key0";
        PlayerSettings.Android.keyaliasPass = "123456";
    }

    [MenuItem("Tools/SetAdmobAndFacebook")]
    public static void SetAdmobAndFacebook()
    {
        var sdkConfigs = GameUtils.GetConfigInfos<SdkConfig>();
        var sdkConfig = sdkConfigs.Find(item=>!item.isTest);
        Debug.LogError($"==> SetAdmobAndFacebook: {JsonConvert.SerializeObject(sdkConfig, Formatting.Indented)}");
        CreateUpdateFacebookSettings(sdkConfig);
        CreateUpdateAdmobSettings(sdkConfig);
    }

    private static void CreateUpdateAdmobSettings(SdkConfig sdkConfig)
    {
        GoogleMobileAdsSettingsEditor.OpenInspector();

        var mobileAdSettingFolder = "Assets/GoogleMobileAds/Resources";
        var MobileAdsSettingsFile = $"{mobileAdSettingFolder}/GoogleMobileAdsSettings.asset";
        var googleMobileSettings = AssetDatabase.LoadAssetAtPath<GoogleMobileAdsSettings>(MobileAdsSettingsFile);

        if (googleMobileSettings == null)
        {
            Directory.CreateDirectory(mobileAdSettingFolder);
            googleMobileSettings = ScriptableObject.CreateInstance<GoogleMobileAdsSettings>();
            AssetDatabase.CreateAsset(googleMobileSettings, MobileAdsSettingsFile);
        }
        
        googleMobileSettings.GoogleMobileAdsAndroidAppId = sdkConfig.admobAppId;
        googleMobileSettings.OptimizeAdLoading = true;
        googleMobileSettings.OptimizeInitialization = true;
        googleMobileSettings.DelayAppMeasurementInit = true;

        EditorUtility.SetDirty(googleMobileSettings);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CreateUpdateFacebookSettings(SdkConfig sdkConfig)
    {
        var facebookAssetFolder = $"Assets/{FacebookSettings.FacebookSettingsPath}";
        var assetPath = $"{facebookAssetFolder}/{FacebookSettings.FacebookSettingsAssetName}{FacebookSettings.FacebookSettingsAssetExtension}";
        var facebookAsset = AssetDatabase.LoadAssetAtPath<FacebookSettings>(assetPath);
        
        if (facebookAsset == null)
        {
            Directory.CreateDirectory(facebookAssetFolder);
            facebookAsset = ScriptableObject.CreateInstance<FacebookSettings>();
            AssetDatabase.CreateAsset(facebookAsset, assetPath);
        }

        FacebookSettings.AppIds[0] = sdkConfig.facebookAppId;
        FacebookSettings.AppLabels[0] = Application.productName;
        FacebookSettings.ClientTokens[0] = sdkConfig.facebookToken;

        EditorUtility.SetDirty(facebookAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}