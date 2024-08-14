using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class UIButton : Button
    {
        public List<GameObject> interactableObjects = new List<GameObject>();
        public List<GameObject> notInteractableObjects = new List<GameObject>();
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
            interactableObjects.ForEach(item => item.SetActive(state == SelectionState.Normal));
            notInteractableObjects.ForEach(item => item.SetActive(state == SelectionState.Disabled));
        }
    }
}