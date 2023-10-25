using Cysharp.Threading.Tasks;
using System;
using Framework;
using System.Threading;

namespace GameFramework
{
    public class UIMessage : UIView
    {
        public override void Init(object param = null)
        {
            base.Init(param);
            OnShowAsync().Forget();
        }

        protected override void OnAdListeners()
        {
            base.OnAdListeners();
            this.RegisterEvent<ClearAllMessageEvent>(v => {
                Destroy(gameObject);
            })
            .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        protected async UniTask OnShowAsync()
        {
            var cancelToken = this.GetCancellationTokenOnDestroy();
            await OnShowMessageAsync(cancelToken);
        }

        protected async UniTask OnShowAndCloseAsync()
        {
            var cancelToken = this.GetCancellationTokenOnDestroy();
            await OnShowMessageAsync(cancelToken);
            await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: cancelToken);
            await OnHideMessageAsync(cancelToken);
            Destroy(gameObject);
        }

        public virtual async UniTask OnShowMessageAsync(CancellationToken cancellationToken)
        {
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
        }

        public virtual async UniTask OnHideMessageAsync(CancellationToken cancellationToken)
        {
            await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
        }
    }
}

