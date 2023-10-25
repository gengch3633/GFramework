using UnityEditor;
 
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
}