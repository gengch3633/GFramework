
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset;

namespace GameFramework
{
    public class AssetBundleManager: MonoSingleton<AssetBundleManager>, ITypeLog
    {
		private string packageVersion;
		public bool IsYooAssetInitSuccess = false;
		public async UniTask InitAsync(EPlayMode playMode, string packageName)
		{
			GameUtils.Log(this, $"packageName: {packageName}");
			BetterStreamingAssets.Initialize();
			YooAssets.Initialize();
			YooAssets.SetOperationSystemMaxTimeSlice(30);
			var initPackageStatus = await InitPackageAsync(playMode, packageName);
			GameUtils.Log(this, $"initPackageStatus: {initPackageStatus}");

			var getStaticVersionStatus = await GetStaticVersionAsync(packageName);
			GameUtils.Log(this, $"getStaticVersionStatus: {getStaticVersionStatus}");

			var updateManefistStatus = await UpdateManifestAsync(packageName);
			GameUtils.Log(this, $"updateManefistStatus: {updateManefistStatus}");

			IsYooAssetInitSuccess = initPackageStatus == EOperationStatus.Succeed && getStaticVersionStatus == EOperationStatus.Succeed && updateManefistStatus == EOperationStatus.Succeed;
		}

		private async UniTask<EOperationStatus> InitPackageAsync(EPlayMode playMode, string packageName)
		{
			GameUtils.Log(this, $"playMode: {playMode}, packageName: {packageName}");
			// 创建默认的资源包
			var package = YooAssets.TryGetAssetsPackage(packageName);
			if (package == null)
			{
				package = YooAssets.CreateAssetsPackage(packageName);
				YooAssets.SetDefaultAssetsPackage(package);
			}

			// 编辑器下的模拟模式
			InitializationOperation initializationOperation = null;
			if (playMode == EPlayMode.EditorSimulateMode)
			{
				var createParameters = new EditorSimulateModeParameters();
				createParameters.SimulatePatchManifestPath = EditorSimulateModeHelper.SimulateBuild(packageName);
				initializationOperation = package.InitializeAsync(createParameters);
			}

			// 单机运行模式
			if (playMode == EPlayMode.OfflinePlayMode)
			{
				var createParameters = new OfflinePlayModeParameters();
				createParameters.DecryptionServices = new GameDecryptionServices();
				initializationOperation = package.InitializeAsync(createParameters);
			}

			// 联机运行模式
			if (playMode == EPlayMode.HostPlayMode)
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
			var hostServerIP = "https://du56qcczvyj27.cloudfront.net";
			var gameName = "SolitaireCollectionGirls";
			var versionName = "V120";
			var platformName = GameUtils.IsIosPlatform() ? "IOS" : "Android";
			var serverUrl = $"{hostServerIP}/{gameName}/{versionName}/{platformName}";
			GameUtils.Log(this, "serverUrl:" + serverUrl);
			return serverUrl;
		}

		private async UniTask<EOperationStatus> GetStaticVersionAsync(string packageName)
		{
			var package = YooAssets.GetAssetsPackage(packageName);
			var operation = package.UpdatePackageVersionAsync();
			await operation.ToUniTask();
			packageVersion = operation.PackageVersion;
			GameUtils.Log(this, $"packageVersion: {packageVersion}");
			return operation.Status;
		}

		private async UniTask<EOperationStatus> UpdateManifestAsync(string packageName)
		{
			var package = YooAssets.GetAssetsPackage(packageName);
			var operation = package.UpdatePackageManifestAsync(packageVersion);
			await operation.ToUniTask();
			return operation.Status;
		}
		private class GameDecryptionServices : IDecryptionServices
		{
			public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
			{
				return 32;
			}

			public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
			{
				throw new NotImplementedException();
			}

			public FileStream LoadFromStream(DecryptFileInfo fileInfo)
			{
				BundleStream bundleStream = new BundleStream(fileInfo.FilePath, FileMode.Open);
				return bundleStream;
			}

			public uint GetManagedReadBufferSize()
			{
				return 1024;
			}
		}

		private class GameQueryServices : IQueryServices
		{
			public bool QueryStreamingAssets(string fileName)
			{
				// 注意：使用了BetterStreamingAssets插件，使用前需要初始化该插件！
				string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
				return BetterStreamingAssets.FileExists($"{buildinFolderName}/{fileName}");
			}
		}

		public bool IsTypeLogEnabled()
        {
			return GameUtils.IsTypeLogEnabled(this);
        }
    }
}

