using System;
using Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFramework
{
    public class AudioClick : MonoBaseController, IPointerClickHandler
    {
        public string soundName = "buttons";
        public void OnPointerClick(PointerEventData eventData)
        {
            audioSystem.PlaySound(soundName);
        }
    }
}