using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class ButtonExt : Button
    {
        private List<Graphic> graphics;
        protected override void Awake()
        {
            base.Awake();
            graphics = this.GetComponentsInChildren<Graphic>().ToList();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            //Debug.LogError($"==> [DoStateTransition] {state}, {instant}");

            if (state == SelectionState.Normal)
                graphics.ForEach(item => item.color = colors.normalColor);

            if (state == SelectionState.Disabled)
                graphics.ForEach(item => item.color = colors.disabledColor);
        }
    }
}