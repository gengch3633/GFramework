using YooAsset;

namespace GameFramework
{
    /// <summary>
    /// 内置文件查询服务类
    /// </summary>
    public class GameQueryServices : IQueryServices
    {
        public bool QueryStreamingAssets(string fileName)
        {
            // 注意：使用了BetterStreamingAssets插件，使用前需要初始化该插件！
            string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
            return BetterStreamingAssets.FileExists($"{buildinFolderName}/{fileName}");
        }
    }
}

