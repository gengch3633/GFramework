using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class VideoBtn_BreathAnim : MonoBehaviour
    {
        private string animName = "Video_btn";
        public void Reset()
        {
            AddAnim(gameObject, animName);
        }

        private void AddAnim(GameObject itemGo, string animName)
        {
            Animation animation = itemGo.AddComponent<Animation>();
            animation.playAutomatically = true;
            AnimationClip animClip = Resources.Load<AnimationClip>($"AnimationClips/{animName}");
            animation.AddClip(animClip, animName);
            animation.clip = animClip;
        }
    }
}

