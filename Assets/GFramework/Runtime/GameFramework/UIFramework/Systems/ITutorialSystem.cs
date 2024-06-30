using Cysharp.Threading.Tasks;
using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public interface ITutorialSystem : ISystem
    {
        bool IsTutorialCompleted<T>() where T: Tutorial_Base;
        void CompleteTutorial<T>() where T : Tutorial_Base;

        void ShowFingerTutorial(Transform target, bool showUp = true);
        void CloseFingerTutorial();
    }

    public class TutorialSystem : AbstractSystem, ITutorialSystem
    {
        public BindableProperty<List<string>> TypeLogEnableSwitchGroup { get; } = new BindableProperty<List<string>>() { Value = new List<string>() };

        public bool IsTutorialCompleted<T>() where T : Tutorial_Base
        {
            var tutorialName = typeof(T).FullName;
            var isCompleted = TypeLogEnableSwitchGroup.Value.Contains(tutorialName);
            return isCompleted;
        }

        public void CompleteTutorial<T>() where T : Tutorial_Base
        {
            var tutorialName = typeof(T).FullName;
            if (!TypeLogEnableSwitchGroup.Value.Contains(tutorialName))
            {
                TypeLogEnableSwitchGroup.Value.Add(tutorialName);
                SaveInfo(this);
            }
        }
        private Dictionary<Transform, Transform> tutorialDict = new Dictionary<Transform, Transform>();
        public void ShowFingerTutorial(Transform target, bool showUp = true)
        {
            Vector3 position = target.position;
            var fingerContainer = GameObject.Find("UISystem/Top").transform;
            GameObject mask = new GameObject("TutorialMask", new Type[] { typeof(Image), typeof(CanvasRenderer)});
            mask.transform.SetParent(fingerContainer);
            mask.transform.localScale = Vector3.one;
            mask.transform.position = Vector3.zero;

            mask.GetComponent<Image>().color = new Color(0, 0, 0, 120f / 255);
            mask.GetComponent<RectTransform>().sizeDelta = Vector2.one * 10000;

            GameObject prefab = Resources.Load<GameObject>("Prefabs/GuideFinger");
            GameObject fingerGo = GameObject.Instantiate(prefab, mask.transform);
            fingerGo.transform.localScale = Vector3.one;
            fingerGo.transform.position = position;
            CheckSetFingerPosition(fingerGo, position).Forget();

            tutorialDict.Add(target, mask.transform);
            SetTargetOrder(fingerContainer, target.gameObject, 1);
            SetTargetOrder(fingerContainer, fingerGo, 2);
        }

        private async UniTask CheckSetFingerPosition(GameObject fingerGo, Vector3 position)
        {
            var frameCount = 0;
            while(fingerGo != null && fingerGo.activeInHierarchy && frameCount < 10)
            {
                fingerGo.transform.position = position;
                await UniTask.DelayFrame(1);
                frameCount++;
            }
        }

        private void SetTargetOrder(Transform fingerContainer, GameObject target, int addedOrder)
        {
            var canvas = target.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = target.AddComponent<Canvas>();
                target.AddComponent<GraphicRaycaster>();
            }

            canvas.overrideSorting = true;
            canvas.sortingLayerID = fingerContainer.GetComponentInParent<Canvas>().sortingLayerID;
            canvas.sortingOrder = fingerContainer.GetComponentInParent<Canvas>().sortingOrder + addedOrder;
        }

        public void CloseFingerTutorial()
        {
            foreach(var kv in tutorialDict)
            {
                var target = kv.Key;
                var mask = kv.Value;
                target.GetComponent<Canvas>().overrideSorting = false;
                GameObject.Destroy(mask.gameObject);
            }

            tutorialDict.Clear();
        }
    }
}

