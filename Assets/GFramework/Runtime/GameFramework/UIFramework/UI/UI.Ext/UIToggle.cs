using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    [RequireComponent(typeof(Toggle))]
    public class UIToggle : MonoBehaviour
    {
        public List<GameObject> offObjects;
        public List<GameObject> onObjects;
        

        private void Awake()
        {
            var toggle = GetComponent<Toggle>();
            SetIsOn(toggle.isOn);
            toggle.onValueChanged.AddListener(SetIsOn);
        }

        private void SetIsOn(bool isOn)
        {
            offObjects.ForEach(item => item.SetActive(!isOn));
            onObjects.ForEach(item => item.SetActive(isOn));
        }
    }
}
