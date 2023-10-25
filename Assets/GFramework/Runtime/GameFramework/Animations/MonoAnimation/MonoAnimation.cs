using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [RequireComponent(typeof(Animation))]
    public class MonoAnimation : MonoBehaviour
    {
        private Animation anim;
        private Action onCompleteAction;
        private void Awake()
        {
            this.anim = GetComponent<Animation>();
        }

        public virtual void Reset()
        {
            List<string> allAnimNames = GetAllAnimNames();
            allAnimNames.ForEach(animName => AddAnim(animName));
        }

        public virtual List<string> GetAllAnimNames()
        {
            return new List<string>();
        }

        public virtual void PlayAnim(string animName, Action onComplete = null)
        {
            onCompleteAction = onComplete;
            anim.Play(animName);
            AnimationState animationState = anim[animName];
            AnimationClip animationClip = animationState.clip;

            AnimationEvent animationEvent = new AnimationEvent();
            animationEvent.functionName = "OnAnimCompleteEvent";
            animationEvent.time = animationState.length;
            animationClip.AddEvent(animationEvent);
        }

        private void AddAnim(string animName)
        {
            Animation animation = this.GetComponent<Animation>();
            animation.playAutomatically = false;
            AnimationClip animClip = Resources.Load<AnimationClip>($"AnimationClips/{animName}");
            animation.AddClip(animClip, animName);
        }

        private void OnAnimCompleteEvent()
        {
            onCompleteAction?.Invoke();
            onCompleteAction = null;
        }
    }
}

