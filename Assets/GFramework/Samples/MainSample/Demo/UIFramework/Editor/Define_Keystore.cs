using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameFramework 
{

    [InitializeOnLoad]
    public class Define_Keystore
    {
        static Define_Keystore()
        {
            AutoSetKeyStore(false);
        }

        [MenuItem("Tools/1.AutoSetKeyStore")]
        private static void AutoSetKeyStore(bool logEnabled = true)
        {
            var assetPath = GameUtils.GetAssetPath(Application.dataPath);
            var assetDirInfo = new DirectoryInfo(assetPath);
            var keystoreFiles = assetDirInfo.GetFiles("*.keystore", SearchOption.AllDirectories);
            for (int i = 0; i < keystoreFiles.Length; i++)
            {
                var keystoreFile = keystoreFiles[i];
                if (logEnabled) Debug.LogError($"==> [Define_Keystore] 1, KeyStoreName {i + 1}: {keystoreFile.FullName}");
            }

            var keyStoreFile = GameUtils.GetAssetPath(keystoreFiles[0].FullName);
            if(logEnabled) Debug.LogError($"==> [Define_Keystore] 2, KeyStoreFile: {keyStoreFile}");
            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = keyStoreFile;
            PlayerSettings.Android.keystorePass = "123456";
            PlayerSettings.Android.keyaliasName = "gt";
            PlayerSettings.Android.keyaliasPass = "123456";
        }
    }
}
