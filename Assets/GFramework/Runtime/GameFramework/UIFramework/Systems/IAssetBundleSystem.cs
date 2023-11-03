
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace GameFramework
{
    public partial class IAssetBundleSystem
    {
        private string packageName = "BgResources";
        private EPlayMode PlayMode = EPlayMode.OfflinePlayMode;
        private string packageVersion;

        private string remoteServerIP = "https://www.trumpgamestudio.com/bg";
        public async UniTask OnInitAsync()
        {
            BetterStreamingAssets.Initialize();
            YooAssets.Initialize();
            YooAssets.SetOperationSystemMaxTimeSlice(30);
            var status = await InitPackageAsync();
            status = await GetStaticVersionAsync();
            status = await UpdateManifestAsync();
        }
        
        public PatchDownloaderOperation GetBundleDownloader(string bgName)
        {
            var package = YooAssets.GetAssetsPackage(packageName);
            var assetInfo = package.GetAssetInfo(bgName);
            var assetInfos = new List<AssetInfo>();
            assetInfos.Add(assetInfo);
            var downloader = YooAssets.CreateBundleDownloader(assetInfos.ToArray(), 10, 3);
            return downloader;
        }

        private async UniTask<EOperationStatus> InitPackageAsync()
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

            Debug.LogError("==> serverUrl:" + serverUrl);
            return serverUrl;
        }


        private async UniTask<EOperationStatus> GetStaticVersionAsync()
        {
            var package = YooAssets.GetAssetsPackage(packageName);
            var operation = package.UpdatePackageVersionAsync();
            await operation.ToUniTask();
            packageVersion = operation.PackageVersion;
            if (logInfo) Debug.LogError("=> GetStaticVersion 1111: " + operation.PackageVersion);
            return operation.Status;
        }

        private async UniTask<EOperationStatus> UpdateManifestAsync()
        {
            var package = YooAssets.GetAssetsPackage(packageName);
            var operation = package.UpdatePackageManifestAsync(packageVersion);
            await operation.ToUniTask();
            return operation.Status;
        }
    }
}

