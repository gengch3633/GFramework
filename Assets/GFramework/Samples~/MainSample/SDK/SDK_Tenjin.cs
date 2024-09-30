using GameFramework;

public class SDK_Tenjin
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
        var tenjinSdkKey = SDKConst.sdkConfig.tenjinSdkKey;
        BaseTenjin instance = Tenjin.getInstance(tenjinSdkKey);
        if (!GameUtils.IsIosPlatform())
            instance.SetAppStoreType(AppStoreType.googleplay);
        instance.Connect();
#endif
    }
}