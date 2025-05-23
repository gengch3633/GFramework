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
        void SaveDataToSystemBuffer(string text);
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
            logNames.Sort((item1, item2) => {
                var item1Value = IsTypeLogEnabled(item1) ? 1 : -1;   
                var item2Value = IsTypeLogEnabled(item2) ? 1 : -1;
                return item2Value - item1Value;
            });

            return logNames;
        }

        public void SetTypeLogEnabled(string typeFullName, bool enabled)
        {
            TypeLogEnableSwitchGroup.Value.SetSwitchOn(typeFullName, enabled);
            TypeLogEnableSwitchGroup.Value = TypeLogEnableSwitchGroup.Value;
        }

        public bool IsTypeLogEnabled(string typeFullName)
        {
            var ret = TypeLogEnableSwitchGroup.Value.IsSwitchOn(typeFullName);
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

            var allDebugFeatureNames = subTypeList.ConvertAll(item => item.FullName);
            allDebugFeatureNames.Sort((item1, item2) => {
                var item1Value = IsDebugFeatureEnabled(item1) ? 1 : -1;
                var item2Value = IsDebugFeatureEnabled(item2) ? 1 : -1;
                return item2Value - item1Value;
            });

            return allDebugFeatureNames;
        }
        public void SaveDataToSystemBuffer(string text)
        {
            UnityEngine.GUIUtility.systemCopyBuffer = text;
        }
    }
}

