using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class UIButton : Button
    {
        private List<GameObject> interactableObjects = new List<GameObject>();
        private List<GameObject> notInteractableObjects = new List<GameObject>();
        protected override void Awake()
        {
            base.Awake();
            var childTransforms = GetComponentsInChildren<Transform>().ToList();
            var intTransforms = childTransforms.FindAll(item => item.name.Contains("_Int"));
            var notIntTransforms = childTransforms.FindAll(item => item.name.Contains("_NotInt"));
            interactableObjects.AddRange(intTransforms.ConvertAll(item=>item.gameObject));
            notInteractableObjects.AddRange(notIntTransforms.ConvertAll(item=>item.gameObject));
        }
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            SetBtnsState(state, instant).Forget();
        }
        
        private async UniTask SetBtnsState(SelectionState state, bool instant)
        {
            interactableObjects.ForEach(item => item.SetActive(state != SelectionState.Disabled));
            notInteractableObjects.ForEach(item => item.SetActive(state == SelectionState.Disabled));

            var loopCount = 0;

            while (interactableObjects.Count == 0 || notInteractableObjects.Count == 0)
            {
                await UniTask.DelayFrame(1);
                loopCount++;
                if (loopCount > 5)
                    break;
            }

            interactableObjects.ForEach(item => item.SetActive(state != SelectionState.Disabled));
            notInteractableObjects.ForEach(item => item.SetActive(state == SelectionState.Disabled));
        }
    }
}