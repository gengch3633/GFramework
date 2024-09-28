
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;
using TMPro;
using System;

namespace GameFramework
{
    public class NormalMessage : UIBaseMessage
    {
        public override void Init(object param = null)
        {
            var messageInfo = (MessageInfo)param;
            try
            {
                var languageInfo = messageInfo.GetFormatText(languageSystem);
                infoTextVar.text = languageInfo;
            }
            catch(Exception e)
            {
               Debug.LogError($"==> [NormalMessage] [Init] Exception:\n{e.StackTrace}");
            }
            
            bgVar.gameObject.AddComponent<CanvasGroup>().alpha = 0;
            base.Init(param);
        }

        public override async UniTask OnShowMessageAsync(CancellationToken cancellationToken)
        {
            await bgVar.gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.5f).ToUniTask(cancellationToken:cancellationToken);
        }

        public override async UniTask OnHideMessageAsync(CancellationToken cancellationToken)
        {
            await bgVar.gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.5f).ToUniTask(cancellationToken: cancellationToken);
        }
        private Text infoTextVar;
        private Image bgVar;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            infoTextVar = transform.Find("Bg_Var/InfoText_Var").GetComponent<Text>();
            bgVar = transform.Find("Bg_Var").GetComponent<Image>();

        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();

        }
    }
}


