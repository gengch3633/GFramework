using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.EventSystems;
using System;

namespace GameFramework
{
    public class ComponentCollector
    {
        public static void CreateUIInitMethods(GameObject rootGameObject)
        {
            Dictionary<string, List<string>> actionDict = new Dictionary<string, List<string>>();
            CollectActionsStrings<MonoVarController>(rootGameObject, actionDict);
            CollectActionsStrings<Button>(rootGameObject, actionDict);
            CollectActionsStrings<Toggle>(rootGameObject, actionDict);
            CollectActionsStrings<TMP_InputField>(rootGameObject, actionDict);
            CollectActionsStrings<InputField>(rootGameObject, actionDict);
            CollectActionsStrings<Text>(rootGameObject, actionDict);
            CollectActionsStrings<TextMeshProUGUI>(rootGameObject, actionDict);
            CollectActionsStrings<Image>(rootGameObject, actionDict);
            CollectActionsStrings<ScrollRect>(rootGameObject, actionDict);
            CollectActionsStrings<ParticleSystem>(rootGameObject, actionDict);
            CollectActionsStrings<Animation>(rootGameObject, actionDict);
            CollectActionsStrings<Dropdown>(rootGameObject, actionDict);

            string declareString = string.Join("\n", actionDict["declaration"]);

            string addListenerPre = "protected override void OnAddUIListeners(){base.OnAddUIListeners();";
            string addListenderMethodString = $"{addListenerPre}\n{string.Join("\n", actionDict["initComponent"])}\n{string.Join("\n", actionDict["addListener"])}\n}}";

            string removeListenerPre = "protected override void OnRemoveUIListeners(){base.OnRemoveUIListeners();";
            string removeListenderMethodString = $"{removeListenerPre}\n{string.Join("\n", actionDict["removeListener"])}\n}}";

            string clickMethodString = string.Join("\n", actionDict["clickMethod"]);

            string methodInfo = $"{declareString}\n{addListenderMethodString}\n{removeListenderMethodString}\n{clickMethodString}";
            Debug.LogError("==> Listeners Info:\n" + methodInfo);
        }

        private static Dictionary<string, List<string>> CollectActionsStrings<T>(GameObject rootGameObject, Dictionary<string, List<string>> actionDict) where T : Component
        {
            var components = rootGameObject.GetComponentsInChildren<T>(true).ToList();
            components = components.FindAll(item => item.gameObject != rootGameObject);
            components = components.FindAll(item => (typeof(T).Name != typeof(Animation).Name)
                || (typeof(T).Name == typeof(Animation).Name) && (rootGameObject.GetComponent<UIPopAnim>() == null));
            var endString = "_Var";
            var isTextComp = new List<string>() { typeof(Image).Name, typeof(Text).Name, typeof(TextMeshProUGUI).Name, typeof(MonoVarController).Name }.Contains(typeof(T).Name);
            if (isTextComp)
                components = components.FindAll(item => item.name.Contains(endString));

            components = components.FindAll(item => IsComponentCanBeCollected(rootGameObject, item.gameObject));
            var compCollecors = GameUtils.GetConfigInfos<ComponentInfo>();
            var actionNames = new List<string>() { "declaration", "addListener", "removeListener", "clickMethod", "initComponent" };
            actionNames.ForEach(actionName =>
            {
                var compCollector = compCollecors.Find(item => (item.componentName == typeof(T).Name) && item.actionName == actionName);
                if (compCollector != null)
                {
                    List<string> actionStrings = new List<string> { };
                    components.ForEach(item =>
                    {
                        var compPath = GetCompPath(item, rootGameObject);
                        var replaceComponentName = "";
                        if (typeof(T) == typeof(MonoVarController))
                        {
                            var monoVarController = item.GetComponent<MonoVarController>();
                            replaceComponentName = monoVarController.replaceComponentName;
                        }
                        var actionString = compCollector.CreateString(item.gameObject.name, compPath, replaceComponentName);
                        actionStrings.Add(actionString);
                    });

                    if (!actionDict.ContainsKey(actionName))
                        actionDict[actionName] = new List<string>();
                    actionDict[actionName].AddRange(actionStrings);
                }
            });
            return actionDict;
        }

        private static bool IsComponentCanBeCollected(GameObject rootGo, GameObject componentGo)
        {
            var monoVar = componentGo.GetComponent<MonoVarController>();
            var parentMonoVar = componentGo.GetComponentInParent<MonoVarController>();
            var isParentMonoVar = parentMonoVar == null || rootGo == parentMonoVar.gameObject || monoVar == parentMonoVar;
            return isParentMonoVar;
        }

        private static string GetCompPath(Component item, GameObject rootGameObject)
        {
            List<GameObject> goList = GetPathGameObjects(item, rootGameObject);
            string path = string.Join("/", goList.ConvertAll(item => item.name));
            return path;
        }

        private static List<GameObject> GetPathGameObjects(Component item, GameObject rootGameObject)
        {
            List<GameObject> goList = new List<GameObject>();
            var parentGo = item.gameObject;
            while (parentGo != rootGameObject)
            {
                goList.Add(parentGo);
                parentGo = parentGo.transform.parent.gameObject;
            }
            goList.Reverse();
            return goList;
        }
    }

    public class ComponentInfo
    {
        public string componentName;
        public string actionName;
        public string actionString;

        public string CreateString(string gameObjectName, string compPath, string replaceComponentName = "")
        {
            ComponentInfo copyCompCollecor = JsonConvert.DeserializeObject<ComponentInfo>(JsonConvert.SerializeObject(this));

            var btnName = gameObjectName.Replace("_", "").Replace(" ", "").Replace("(", "").Replace(")", "");
            var btnName1 = btnName.Substring(0, 1).ToLower() + btnName.Substring(1, btnName.Length - 1);
            var btnName2 = btnName.Substring(0, 1).ToUpper() + btnName.Substring(1, btnName.Length - 1);
            var newActionString = copyCompCollecor.actionString;
            newActionString = newActionString.Replace("{btnName1}", btnName1);
            newActionString = newActionString.Replace("{btnName2}", btnName2);
            var realComponentName = string.IsNullOrEmpty(replaceComponentName) ? componentName : replaceComponentName;
            newActionString = newActionString.Replace("{componentName}", realComponentName);
            newActionString = newActionString.Replace("{compPath}", compPath);
            newActionString = newActionString.Replace("\\n", "\n");
            return newActionString;
        }
    }
}

