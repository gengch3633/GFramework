
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;
using TMPro;

namespace GameFramework
{
    public class NormalMessage : UIBaseMessage
    {
        public override void Init(object param = null)
        {
            var messageInfo = (MessageInfo)param;
            var languageInfo = messageInfo.GetFormatText(languageSystem);
            infoTextVar.text = languageInfo;
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
        private TextMeshProUGUI infoTextVar;
        private Image bgVar;
        protected override void OnAddUIListeners()
        {
            base.OnAddUIListeners();
            infoTextVar = transform.Find("Bg_Var/InfoText_Var").GetComponent<TextMeshProUGUI>();
            bgVar = transform.Find("Bg_Var").GetComponent<Image>();

        }
        protected override void OnRemoveUIListeners()
        {
            base.OnRemoveUIListeners();

        }
    }
}


