using System;
using UnityEngine;

namespace GameFramework
{
    public static class AnimExtention
    {
        public static void PlayAnim(this Animation anim, string animName, Action onCompleted, bool playAnim = true)
        {
            if (playAnim) anim.Play(animName);
            RegisterAnimEvent(anim, animName, onCompleted);
        }

        public static void RegisterAnimEvent(this Animation anim, string animName, Action onCompleted)
        {
            var animTrigger = anim.gameObject.GetComponent<AnimCompleteTrigger>();
            animTrigger = animTrigger ?? anim.gameObject.AddComponent<AnimCompleteTrigger>();
            animTrigger.RegisterAnimCompleteEvent(anim, animName, onCompleted);
        }
    }
}


