using System.Collections;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

namespace GameFramework
{
    public class LoadingPanel : UIPanel
    {
		protected override void MonoStart()
        {
            base.MonoStart();
			LoadingSceneAsync().Forget();
			
		}


        protected override void OnDestroy()
        {
            base.OnDestroy();
			adsSystem.ShowBanner();
		}

        private bool IsAnalysisSdkInited()
		{
            var isFirebaseInited = firebaseSystem.IsFirebaseInitialized();
            var isAfInited = AppsFlyerManager.ConversionSuccessFlag;
            var isInited = isFirebaseInited && isAfInited;

            //Debug.LogError($"==> [LoadingPanel], isAfInited: {isAfInited}, isAfInited: {isAfInited}");
            return isInited;
		}

		private async UniTask LoadingSceneAsync()
		{
			loadingBarVar.fillAmount = 0.0f;
			var waitSeconds = 10;
			var startTime = Time.time;


			while (!IsAnalysisSdkInited() && Time.time - startTime < waitSeconds)
			{
				loadingBarVar.fillAmount += 0.01f;
				await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
			}

            if (firebaseSystem.IsFirebaseInitialized())
            {
				Debug.LogError($"==> [LoadingPanel], LogALoadingSceneEvent 1");
				var isNetNotAvailable = Application.internetReachability == NetworkReachability.NotReachable;
				var net = !isNetNotAvailable ? "Y" : "N";
				EventUtils.LogALoadingSceneEvent(net);
				Debug.LogError($"==> [LoadingPanel], LogALoadingSceneEvent 2");
			}

			while (this.loadingBarVar.fillAmount < 1f)
			{
				loadingBarVar.fillAmount += 0.01f;
				await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
			}

			uiSystem.ClosePanel(gameObject);

			EventUtils.LogAGameSceneEvent();
			uiSystem.OpenPanel(PanelType.GamePanel);
		}

		private Image loadingBarVar;
		protected override void OnAddUIListeners()
		{
			base.OnAddUIListeners();
			loadingBarVar = transform.Find("LoadingBar_Var").GetComponent<Image>();

		}
		protected override void OnRemoveUIListeners()
		{
			base.OnRemoveUIListeners();

		}
    }
}

