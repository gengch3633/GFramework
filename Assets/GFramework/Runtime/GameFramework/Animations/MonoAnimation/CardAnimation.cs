using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameFramework
{

    public class CardAnimation : MonoAnimation
    {
        private string shakeAnim = "card_shake";
        private string bounceAnim = "card_bounce";
        private string idleAnim = "card_idle";

        public override List<string> GetAllAnimNames()
        {
            string[] allAnimNames = new string[] { shakeAnim, bounceAnim, idleAnim };
            return allAnimNames.ToList();
        }

        public void PlayShakeAnim()
        {
            PlayAnim(shakeAnim);
        }

        public void PlayBounceAnim()
        {
            PlayAnim(bounceAnim);
        }

        public void PlayIdleAnim()
        {
            PlayAnim(idleAnim);
        }
    }
}
