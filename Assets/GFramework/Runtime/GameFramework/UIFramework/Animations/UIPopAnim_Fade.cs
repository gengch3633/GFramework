using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class UIPopAnim_Fade : UIPopAnim
    {
        protected override string GetPopOutAnimName()
        {
            return "Popout_Fade";
        }

        protected override string GetPopInAnimName()
        {
            return "Popin_Fade";
        }
    }
}

