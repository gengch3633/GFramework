using UnityEngine;

namespace GameFramework
{
    [RequireComponent(typeof(Animation))]
    public class ClickBtnAni_Big : ClickBtnAni_Base
    {
        protected override void PlayBtnDownAnim()
        {
            base.PlayBtnDownAnim();
            scaleAni.Play(btnDownBigAnimName);
        }

        protected override void PlayBtnUpAnim()
        {
            base.PlayBtnUpAnim();
            scaleAni.Play(btnUpBigAnimName);
        }
    }
}

