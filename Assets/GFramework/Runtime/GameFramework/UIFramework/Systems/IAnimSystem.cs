using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace GameFramework
{
    public interface IAnimSystem : ISystem
    {

    }
    public class AnimSystem : AbstractSystem, IAnimSystem
    {
        public async UniTask MoveFrom(RectTransform rectTransform)
        {
            var width = rectTransform.sizeDelta.x;
            
        }

        public async UniTask MoveTo()
        {

        }
    }
}

