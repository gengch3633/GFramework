using UnityEngine;

namespace GameFramework
{
    [RequireComponent(typeof(Animation))]
    public class ClickBtnAni_Small : ClickBtnAni_Base
    {
        protected override void PlayBtnDownAnim()
        {
            base.PlayBtnDownAnim();
            scaleAni.Play(btnDownSmallAnimName);
        }

        protected override void PlayBtnUpAnim()
        {
            base.PlayBtnUpAnim();
            scaleAni.Play(btnUpSmallAnimName);
        }
    }
}

