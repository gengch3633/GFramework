using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace GameFramework
{
    public partial class MonoController
    {
        protected IAdsSystem adsSystem;
        protected IEventSystem eventSystem;
        protected override void MonoAwakePartial()
        {
            base.MonoAwakePartial();
            adsSystem = this.GetSystem<IAdsSystem>();
            eventSystem = this.GetSystem<IEventSystem>();
        }
    }
}

