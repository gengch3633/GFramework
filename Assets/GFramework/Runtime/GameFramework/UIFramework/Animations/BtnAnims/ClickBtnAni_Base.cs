
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameFramework
{
    [RequireComponent(typeof(Animation))]
    public class ClickBtnAni_Base : Button
    {
        protected string btnDownSmallAnimName = "btnstuffdown_small";
        protected string btnUpSmallAnimName = "btnstuffup_small";
        protected string btnDownBigAnimName = "btnstuffdown_big";
        protected string btnUpBigAnimName = "btnstuffup_big";

        protected Animation scaleAni;
        private bool isDown;

        protected override void Awake()
        {
            base.Awake();
            this.scaleAni = this.GetComponent<Animation>();
        }
        protected override void OnDisable()
        {
            this.transform.localScale = Vector3.one;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            AddButtonAnims();
        }
        public void AddButtonAnims()
        {
            this.transition = Transition.None;
            Animation animation = this.GetComponent<Animation>();
            GetComponent<Animation>().playAutomatically = false;
            AddAnim(animation, btnDownSmallAnimName);
            AddAnim(animation, btnUpSmallAnimName);
            AddAnim(animation, btnDownBigAnimName);
            AddAnim(animation, btnUpBigAnimName);
        }
#endif

        public override void OnPointerDown(PointerEventData pointerEventData)
        {
            base.OnPointerDown(pointerEventData);
            this.isDown = true;
            this.scaleAni.Stop();
            PlayBtnDownAnim();
        }

        protected virtual void PlayBtnDownAnim()
        {
            this.scaleAni.Play(btnDownSmallAnimName);
        }

        public override void OnPointerUp(PointerEventData pointerEventData)
        {
            base.OnPointerUp(pointerEventData);
            if (!this.isDown) return;

            this.isDown = false;
            this.scaleAni.Stop();
            PlayBtnUpAnim();
        }

        protected virtual void PlayBtnUpAnim()
        {
            this.scaleAni.Play(btnUpSmallAnimName);
        }

        protected void AddAnim(Animation animation, string animName)
        {
            AnimationClip upAnimClip = Resources.Load<AnimationClip>($"AnimationClips/{animName}");
            animation.AddClip(upAnimClip, animName);
        }
    }
}
