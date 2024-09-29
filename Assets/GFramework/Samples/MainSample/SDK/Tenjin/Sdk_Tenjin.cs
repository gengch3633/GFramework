using GameFramework;

public class Sdk_Tenjin
{
    public static void Init()
    {
        TenjinConnect();
    }

    public static void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
            TenjinConnect();
    }

    private static void TenjinConnect()
    {
#if SDK_TENJIN
        var tenjinSdkKey = SDKConst.SdkConfigProduction.tenjinSdkKey;
        BaseTenjin instance = Tenjin.getInstance(tenjinSdkKey);
        if (!GameUtils.IsIosPlatform())
            instance.SetAppStoreType(AppStoreType.googleplay);
        instance.Connect();
#endif
    }
}