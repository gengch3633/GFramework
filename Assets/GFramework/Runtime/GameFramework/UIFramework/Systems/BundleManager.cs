
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset;

namespace GameFramework
{
    public class BundleManager: MonoSingleton<BundleManager>
    {
		private string packageVersion;
		public bool IsYooAssetInitSuccess = false;
		public async UniTask InitAsync(EPlayMode playMode, string packageName)
		{
			BetterStreamingAssets.Initialize();
			YooAssets.Initialize();
			YooAssets.SetOperationSystemMaxTimeSlice(30);
			var initPackageStatus = await InitPackageAsync(playMode, packageName);
			if (IsTypeLogEnabled()) Debug.LogError($"==> [PatchSystem] InitPackageAsync: {initPackageStatus}");

			var getStaticVersionStatus = await GetStaticVersionAsync(packageName);
			if (IsTypeLogEnabled()) Debug.LogError($"==> [PatchSystem] GetStaticVersionAsync: {getStaticVersionStatus}");

			var updateManefistStatus = await UpdateManifestAsync(packageName);
			if (IsTypeLogEnabled()) Debug.LogError($"==> [PatchSystem] UpdateManifestAsync: {updateManefistStatus}");

			IsYooAssetInitSuccess = initPackageStatus == EOperationStatus.Succeed && getStaticVersionStatus == EOperationStatus.Succeed && updateManefistStatus == EOperationStatus.Succeed;
		}

		private async UniTask<EOperationStatus> InitPackageAsync(EPlayMode playMode, string packageName)
		{
			if (IsTypeLogEnabled()) Debug.LogError($"==> [PatchSystem] playMode: {playMode}");
			// ����Ĭ�ϵ���Դ��
			var package = YooAssets.TryGetAssetsPackage(packageName);
			if (package == null)
			{
				package = YooAssets.CreateAssetsPackage(packageName);
				YooAssets.SetDefaultAssetsPackage(package);
			}

			// �༭���µ�ģ��ģʽ
			InitializationOperation initializationOperation = null;
			if (playMode == EPlayMode.EditorSimulateMode)
			{
				var createParameters = new EditorSimulateModeParameters();
				createParameters.SimulatePatchManifestPath = EditorSimulateModeHelper.SimulateBuild(packageName);
				initializationOperation = package.InitializeAsync(createParameters);
			}

			// ��������ģʽ
			if (playMode == EPlayMode.OfflinePlayMode)
			{
				var createParameters = new OfflinePlayModeParameters();
				createParameters.DecryptionServices = new GameDecryptionServices();
				initializationOperation = package.InitializeAsync(createParameters);
			}

			// ��������ģʽ
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
			if(IsTypeLogEnabled()) Debug.LogError("==> [PatchSystem] serverUrl:" + serverUrl);
			return serverUrl;
		}

		private async UniTask<EOperationStatus> GetStaticVersionAsync(string packageName)
		{
			var package = YooAssets.GetAssetsPackage(packageName);
			var operation = package.UpdatePackageVersionAsync();
			await operation.ToUniTask();
			packageVersion = operation.PackageVersion;
			if (IsTypeLogEnabled()) Debug.LogError("=> [PatchSystem] GetStaticVersion: " + operation.PackageVersion);
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
				// ע�⣺ʹ����BetterStreamingAssets�����ʹ��ǰ��Ҫ��ʼ���ò����
				string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
				return BetterStreamingAssets.FileExists($"{buildinFolderName}/{fileName}");
			}
		}

		public bool IsTypeLogEnabled()
        {
			var debugModel = GameApp.Interface.GetModel<IDebugModel>();
			var ret = debugModel.IsTypeLogEnabled(this);
			return ret;
        }
    }
}

