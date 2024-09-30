using System.Collections;
using System.Collections.Generic;
#if SDK_FB
using Facebook.Unity;
#endif
using UnityEngine;
namespace GameFramework
{
    public class SDK_FB
    {
        public static void Init()
        {
#if SDK_FB
            if (FB.IsInitialized)
                FB.ActivateApp();
            else
                FB.Init(FB.ActivateApp);
#endif
        }
    }
}

