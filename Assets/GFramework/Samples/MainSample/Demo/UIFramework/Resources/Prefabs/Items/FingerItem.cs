
using UnityEngine;

namespace GameFramework
{
    public class FingerItem : MonoController
    {
        public GameObject ring1;
        public GameObject ring2;

        public void Init(bool playTapAnim)
        {
            ring1.gameObject.SetActive(playTapAnim);
            ring2.gameObject.SetActive(playTapAnim);

            if(playTapAnim) GetComponent<Animator>().enabled = true;
        }

    }
}

