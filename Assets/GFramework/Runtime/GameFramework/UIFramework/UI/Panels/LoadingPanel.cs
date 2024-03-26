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

       

		private async UniTask LoadingSceneAsync()
		{
			loadingBarVar.fillAmount = 0.0f;
			var waitSeconds = 10;
			var startTime = Time.time;

			while (this.loadingBarVar.fillAmount < 1f)
			{
				loadingBarVar.fillAmount += 0.01f;
				await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
			}

			uiSystem.ClosePanel(gameObject);

			EventUtils.LogAGameSceneEvent();
			//uiSystem.OpenPanel(PanelType.GamePanel);
			uiSystem.OpenPopup<DebugPopup>();
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

