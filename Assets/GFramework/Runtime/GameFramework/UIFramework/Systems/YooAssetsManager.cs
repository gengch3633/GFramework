
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace GameFramework
{
    public class YooAssetsManager:MonoSingleton<YooAssetsManager>
    {
        private bool logInfo = false;
        private EPlayMode PlayMode = EPlayMode.OfflinePlayMode;
        private string remoteServerIP = "https://www.trumpgamestudio.com/bg";
        private List<string> allPackageNames = new List<string>() { "card" };
        public async UniTask InitAsync(bool logInfo = false)
        {
            this.logInfo = logInfo;
            this.PlayMode = GameUtils.IsEditor() ? EPlayMode.EditorSimulateMode : PlayMode;
            BetterStreamingAssets.Initialize();
            YooAssets.Initialize();
            YooAssets.SetOperationSystemMaxTimeSlice(30);
            for (int i = 0; i < allPackageNames.Count; i++)
            {
                var packageName = allPackageNames[i];
                await InitializePackageAsync(packageName);
                await UpdateManifestAsync(packageName);
            }
        }
        
        public PatchDownloaderOperation GetBundleDownloader(string packageName, string bgName)
        {
            var package = YooAssets.GetAssetsPackage(packageName);
            var assetInfo = package.GetAssetInfo(bgName);
            var assetInfos = new List<AssetInfo>();
            assetInfos.Add(assetInfo);
            var downloader = YooAssets.CreateBundleDownloader(assetInfos.ToArray(), 10, 3);
            return downloader;
        }

        private async UniTask<EOperationStatus> InitializePackageAsync(string packageName)
        {
            // 创建默认的资源包
            var package = YooAssets.TryGetAssetsPackage(packageName);
            if (package == null)
            {
                package = YooAssets.CreateAssetsPackage(packageName);
                YooAssets.SetDefaultAssetsPackage(package);
            }

            // 编辑器下的模拟模式
            InitializationOperation initializationOperation = null;
            if (PlayMode == EPlayMode.EditorSimulateMode)
            {
                var createParameters = new EditorSimulateModeParameters();
                createParameters.SimulatePatchManifestPath = EditorSimulateModeHelper.SimulateBuild(packageName);
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 单机运行模式
            if (PlayMode == EPlayMode.OfflinePlayMode)
            {
                var createParameters = new OfflinePlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                initializationOperation = package.InitializeAsync(createParameters);
            }

            // 联机运行模式
            if (PlayMode == EPlayMode.HostPlayMode)
            {
                var createParameters = new HostPlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                createParameters.QueryServices = new GameQueryServices();
                createParameters.DefaultHostServer = GetHostServerURL();
                createParameters.FallbackHostServer = GetHostServerURL();
                initializationOperation = package.InitializeAsync(createParameters);
            }

            await initializationOperation.ToUniTask();
            return package.InitializeStatus;
        }

        private string GetHostServerURL()
        {
            var runTimePlatform = Application.platform;
            string hostServerIP = remoteServerIP;
            string resVersion = "1.0.0";

            var serverUrl = $"{hostServerIP}/CDN/{runTimePlatform}/{resVersion}";
            if (this.logInfo) Debug.LogError("==> serverUrl:" + serverUrl);
            return serverUrl;
        }

        private async UniTask UpdateManifestAsync(string packageName)
        {
            var package = YooAssets.GetAssetsPackage(packageName);
            var updateVersionOperation = package.UpdatePackageVersionAsync();
            await updateVersionOperation.ToUniTask();
            if (this.logInfo) Debug.LogError("==> [IAssetBundleSystem] updateVersionOperation: " + updateVersionOperation.Status);
            var packageVersion = updateVersionOperation.PackageVersion;
            var updateManefistOperation = package.UpdatePackageManifestAsync(packageVersion);
            await updateManefistOperation.ToUniTask();
            if (this.logInfo) Debug.LogError("==> [IAssetBundleSystem] updateManefistOperation: " + updateManefistOperation.Status);
        }

        public bool IsTypeLogEnabled()
        {
            throw new System.NotImplementedException();
        }
    }
}

