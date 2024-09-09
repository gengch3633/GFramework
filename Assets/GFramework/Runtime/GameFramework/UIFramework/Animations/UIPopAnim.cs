using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{

    [RequireComponent(typeof(Animation))]
    public class UIPopAnim : MonoBehaviour
    {
        private Animation anim;

        protected virtual string GetPopOutAnimName()
        {
            return "Popout";
        }

        protected virtual string GetPopInAnimName()
        {
            return "Popin";
        }


        protected void Awake()
        {
            this.anim = this.GetComponent<Animation>();
        }
        protected void Reset()
        {
            FitToAnimHierarchy();
            AddAnim(GetPopOutAnimName());
            AddAnim(GetPopInAnimName());
        }

        private void FitToAnimHierarchy()
        {
            if (transform.Find("Pop") == null)
            {
                List<Transform> allChildTransforms = new List<Transform>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    allChildTransforms.Add(transform.GetChild(i));
                }

                GameObject goPop = new GameObject("Pop");
                goPop.AddComponent<Image>();
                goPop.GetComponent<Image>().enabled = false;
                goPop.AddComponent<CanvasGroup>();
                goPop.transform.SetParent(transform);
                goPop.transform.SetAsLastSibling();

                RectTransform popTransform = goPop.GetComponent<RectTransform>();

                popTransform.anchorMin = Vector2.zero;
                popTransform.anchorMax = Vector2.one;

                popTransform.offsetMin = Vector2.zero;
                popTransform.offsetMax = Vector2.zero;

                foreach (var item in allChildTransforms)
                {
                    item.SetParent(goPop.transform);
                }
            }

            if (transform.Find("Bg") == null)
            {
                GameObject goBg = new GameObject("Bg");
                Image image = goBg.AddComponent<Image>();
                image.color = Color.black;
                goBg.transform.SetParent(transform);
                goBg.transform.SetAsFirstSibling();

                RectTransform bgTransform = goBg.GetComponent<RectTransform>();

                bgTransform.anchorMin = Vector2.zero;
                bgTransform.anchorMax = Vector2.one;

                bgTransform.offsetMin = -Vector2.one * 1000;
                bgTransform.offsetMax = Vector2.one * 1000;
            }
        }

        private void AddAnim(string animName)
        {
            Animation animation = this.GetComponent<Animation>();
            animation.playAutomatically = false;
            AnimationClip animClip = Resources.Load<AnimationClip>($"AnimationClips/{animName}");
            animation.AddClip(animClip, animName);

        }

        private Action popInAnimCompleteEvent;
        private void OnPopInAnimCompleteEvent()
        {
            popInAnimCompleteEvent?.Invoke();
            popInAnimCompleteEvent = null;
        }

        public void PlayPopinAim(Action onComplete = null)
        {
            var animName = GetPopInAnimName();
            var functionName = "OnPopInAnimCompleteEvent";
            popInAnimCompleteEvent = onComplete;
            PlayAnim(animName, functionName);
        }

        public void ResetPopinAim()
        {
            var animName = GetPopInAnimName();
            anim.Play(animName);
            anim[animName].time = 0;
            anim.Sample();
            anim[animName].enabled = false;
        }

        private Action popOutAnimCompleteEvent;

        private void OnPopOutAnimCompleteEvent()
        {
            popOutAnimCompleteEvent?.Invoke();
            popOutAnimCompleteEvent = null;
        }

        public void PlayPopoutAim(Action onComplete = null)
        {
            var animName = GetPopOutAnimName();
            var functionName = "OnPopOutAnimCompleteEvent";
            popOutAnimCompleteEvent = onComplete;
            PlayAnim(animName, functionName);
        }

        private void PlayAnim(string animName, string functionName)
        {
            anim.Play(animName);
            AnimationClip animationClip = anim.GetClip(animName);

            AnimationEvent animationEvent = new AnimationEvent();
            animationEvent.functionName = functionName;
            animationEvent.time = anim[animName].length;
            animationClip.AddEvent(animationEvent);
        }

    }

    public class AnimParam
    {
        public Animation anim;
        public string animName;

        public Action animCompleteEvent;

        public AnimParam(Animation anim, string animName)
        {
            this.anim = anim;
            this.animName = animName;
        }

        private Action popInAnimCompleteEvent;

        public void PlayAim(Action onComplete = null)
        {
            popInAnimCompleteEvent = onComplete;

            anim.Play(animName);
            AnimationClip animationClip = anim.GetClip(animName);

            AnimationEvent animationEvent = new AnimationEvent();
            animationEvent.functionName = "OnAnimCompleteEvent";
            animationEvent.time = anim[animName].length;
            animationClip.AddEvent(animationEvent);
        }

        private void OnAnimCompleteEvent()
        {
            popInAnimCompleteEvent?.Invoke();
            popInAnimCompleteEvent = null;
        }
    }
}

