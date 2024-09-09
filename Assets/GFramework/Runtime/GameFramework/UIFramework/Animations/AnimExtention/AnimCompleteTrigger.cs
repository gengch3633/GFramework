using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class AnimCompleteTrigger : MonoBehaviour
    {
        private Animation anim;
        private string animName;
        private Action animCompleteEvent;
        public void RegisterAnimCompleteEvent(Animation anim, string animName, Action animCompleteEvent)
        {
            this.anim = anim;
            this.animName = animName;
            this.animCompleteEvent = animCompleteEvent;

            AnimationClip animationClip = anim.GetClip(animName);
            AnimationEvent animationEvent = new AnimationEvent();
            animationEvent.functionName = "OnAnimCompleteEvent";
            animationEvent.time = anim[animName].length;
            animationClip.AddEvent(animationEvent);
        }

        private void OnAnimCompleteEvent()
        {
            //Debug.LogError("==> OnAnimCompleteEvent: " + gameObject.name, gameObject);
            animCompleteEvent?.Invoke();
            animCompleteEvent = null;

            AnimationClip animationClip = anim.GetClip(animName);
            animationClip.events = new List<AnimationEvent>().ToArray();
        }
    }
}

