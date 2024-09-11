using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameFramework 
{

    [InitializeOnLoad]
    public class KeystoreSetter
    {
        static KeystoreSetter()
        {
            var assetPath = GameUtils.GetAssetPath(Application.dataPath);
            var assetDirInfo = new DirectoryInfo(assetPath);
            var keystoreFiles = assetDirInfo.GetFiles("*.keystore", SearchOption.AllDirectories);
            for (int i = 0; i < keystoreFiles.Length; i++)
            {
                var keystoreFile = keystoreFiles[i];
                Debug.LogError($"==> [KeystoreSetter] KeyStoreName {i+1}: " + keystoreFile.FullName);
            }

            var keyStoreFile = GameUtils.GetAssetPath(keystoreFiles[0].FullName);
            Debug.LogError($"==> [KeystoreSetter] keyStoreFile: {keyStoreFile}");
            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = keyStoreFile;
            PlayerSettings.Android.keystorePass = "123456";
            PlayerSettings.Android.keyaliasName = "gt";
            PlayerSettings.Android.keyaliasPass = "123456";
        }
    }
}
