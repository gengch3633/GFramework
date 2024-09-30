using Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameFramework
{
    public interface IDebugModel : IModel
    {
        void RecoverGameDataFromFile();
        void CopyGameData();

        List<string> GetAllDebugFeatureNames();
        List<string> GetAllTypeLogNames();

        bool IsTypeLogEnabled(string typeFullName);
        bool IsTypeLogEnabled<T>(T t) where T:ITypeLog;
        bool IsTypeLogEnabled<T>() where T : ITypeLog;
        void SetTypeLogEnabled(string typeFullName, bool enabled);

        bool IsDebugFeatureEnabled(string debugFeatureName);
        bool IsDebugFeatureEnabled<T>() where T: Debug_Base;
        void SetDebugFeatureEnabled(string debugFeatureName, bool enabled);

        BindableProperty<SwitchGroup> TypeLogEnableSwitchGroup { get; }
        BindableProperty<SwitchGroup> DebugFeatureEnableSwitchGroup { get; }
    }

    public partial class DebugModel : AbstractModel, IDebugModel
    {
        public BindableProperty<SwitchGroup> TypeLogEnableSwitchGroup { get; } = new BindableProperty<SwitchGroup>() { Value = new SwitchGroup() };
        public BindableProperty<SwitchGroup> DebugFeatureEnableSwitchGroup { get; } = new BindableProperty<SwitchGroup>() { Value = new SwitchGroup() };

        protected override void OnInit()
        {
            base.OnInit();
            var blockModel = ReadInfoWithReturnNew<DebugModel>();
            CopyBindableClass(this, blockModel, () => { SaveInfo(this); });

            CheckInitSwitchGroup();
        }

        private void CheckInitSwitchGroup()
        {
            if (TypeLogEnableSwitchGroup.Value.switchInfos.Count <= 0)
            {
                foreach (var logTypeName in GetAllTypeLogNames())
                    TypeLogEnableSwitchGroup.Value.switchInfos.Add(new SwitchInfo() { switchName = logTypeName, isOn = false });

                foreach (var item in GetAllDebugFeatureNames())
                    DebugFeatureEnableSwitchGroup.Value.switchInfos.Add(new SwitchInfo() { switchName = item, isOn = false });

                TypeLogEnableSwitchGroup.Value = TypeLogEnableSwitchGroup.Value;
                DebugFeatureEnableSwitchGroup.Value = DebugFeatureEnableSwitchGroup.Value;
            }
        }

        private List<Type> GetFileSaverTypes()
        {
            var currentNameSpaceTypes = Assembly.GetExecutingAssembly().GetTypes();
            var allTypes = new List<Type>();
            allTypes.AddRange(currentNameSpaceTypes);
            allTypes = allTypes.FindAll(item => item.IsSubclassOf(typeof(FileSaver)));
            return allTypes;
        }

        public List<string> GetAllTypeLogNames()
        {
            var allTypes = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            assemblies.ForEach(assembly =>
            {
                var currentNameSpaceTypes = assembly.GetTypes();
                allTypes.AddRange(currentNameSpaceTypes);
                allTypes = allTypes.FindAll(item => item.GetInterface(typeof(ITypeLog).Name) != null);

                var childClasses = new List<Type>();
                allTypes.ForEach(childClass =>
                {
                    var parentClasses = allTypes.FindAll(item => childClass.IsSubclassOf(item));
                    if (parentClasses.Count > 0)
                        childClasses.Add(childClass);
                });

                childClasses.ForEach(item => allTypes.Remove(item));
            });
            var logNames = allTypes.ConvertAll(item => item.FullName);
            logNames.Sort();

            return logNames;
        }

        public void SetTypeLogEnabled(string typeFullName, bool enabled)
        {
            TypeLogEnableSwitchGroup.Value.SetSwitchOn(typeFullName, enabled);
            TypeLogEnableSwitchGroup.Value = TypeLogEnableSwitchGroup.Value;
            Debug.LogError($"==> [SetTypeLogEnabled], {typeFullName}: {enabled}");
        }

        public bool IsTypeLogEnabled(string typeFullName)
        {
            var ret = TypeLogEnableSwitchGroup.Value.IsSwitchOn(typeFullName);
            Debug.LogError($"==> [IsTypeLogEnabled], {typeFullName}: {ret}");
            return ret;
        }

        public bool IsTypeLogEnabled<T>(T t) where T : ITypeLog
        {
            return IsTypeLogEnabled<T>();
        }

        public bool IsTypeLogEnabled<T>() where T : ITypeLog
        {
            var typeFullName = typeof(T).FullName;
            var ret = IsTypeLogEnabled(typeFullName);
            return ret;
        }

        public void SetDebugFeatureEnabled(string debugFeatureName, bool enabled)
        {
            DebugFeatureEnableSwitchGroup.Value.SetSwitchOn(debugFeatureName, enabled);
            DebugFeatureEnableSwitchGroup.Value = DebugFeatureEnableSwitchGroup.Value;
        }

        public bool IsDebugFeatureEnabled<T>() where T: Debug_Base
        {
            var debugFeatureName = typeof(T).FullName;
            bool ret = IsDebugFeatureEnabled(debugFeatureName);
            return ret;
        }
        public bool IsDebugFeatureEnabled(string debugFeatureName)
        {
            bool ret = DebugFeatureEnableSwitchGroup.Value.IsSwitchOn(debugFeatureName);
            return ret;
        }
        public List<string> GetAllDebugFeatureNames()
        {
            var subTypeList = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            assemblies.ForEach(assembly =>
            {
                var assemblyAllTypes = assembly.GetTypes();
                foreach (var itemType in assemblyAllTypes)
                {
                    var baseType = itemType.BaseType;
                    if (baseType != null)
                    {
                        if (baseType.FullName == typeof(Debug_Base).FullName)
                            subTypeList.Add(itemType);
                    }
                }
            });

            return subTypeList.ConvertAll(item=>item.FullName);
        }

        public void CopyGameData()
        {
            var allFileSaverClasses = GetFileSaverTypes();
            Dictionary<string, string> fileSaverDict = new Dictionary<string, string>();
            allFileSaverClasses.ForEach(item =>
            {
                var playerPrefsKey = item.FullName;
                if (PlayerPrefs.HasKey(playerPrefsKey))
                {
                    var tempData = PlayerPrefs.GetString(playerPrefsKey);
                    fileSaverDict.Add(playerPrefsKey, tempData);
                }
            });

            CopyDataToSystemBuffer(JsonConvert.SerializeObject(fileSaverDict));
        }

        public void RecoverGameDataFromFile()
        {
            var testAsset = Resources.Load<TextAsset>("Data/GameDataRecovery");
            if (testAsset != null)
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(testAsset.text);
                foreach (var item in dict)
                {
                    var prefKey = item.Key;
                    var prefValue = item.Value;
                    PlayerPrefs.SetString(prefKey, prefValue);
                }
            }
        }

        public void CopyDataToSystemBuffer(string text)
        {
            UnityEngine.GUIUtility.systemCopyBuffer = text;
        }
    }
}

