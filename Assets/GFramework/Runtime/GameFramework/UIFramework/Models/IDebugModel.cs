using Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameFramework
{
    public enum EDebugFeature
    {
        MobileTestAds,
        NoAds,
        TimeScaleDown,
        OpenBackDoor
    }
    public interface ITypeLog
    {
        bool IsTypeLogEnabled();
    }
    public interface IDebugModel : IModel
    {
        void RecoverGameDataFromFile();
        void CopyGameData();

        List<string> GetAllDebugFeatureNames();
        List<string> GetAllTypeLogNames();

        bool IsTypeLogEnabled(string typeFullName);
        void SetTypeLogEnabled(string typeFullName, bool enabled);
       
        void SetDebugFeatureEnabled(string debugFeatureName, bool enabled);
        bool IsDebugFeatureEnabled(string debugFeatureName);
        void SetDebugFeatureEnabled(EDebugFeature debugFeature, bool enabled);
        bool IsDebugFeatureEnabled(EDebugFeature debugFeature);
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
                foreach (Type item in GetLogTypes())
                    TypeLogEnableSwitchGroup.Value.switchInfos.Add(new SwitchInfo() { switchName = item.FullName, isOn = false });

                foreach (EDebugFeature item in System.Enum.GetValues(typeof(EDebugFeature)))
                    DebugFeatureEnableSwitchGroup.Value.switchInfos.Add(new SwitchInfo() { switchName = item.ToString(), isOn = false });

                TypeLogEnableSwitchGroup.Value = TypeLogEnableSwitchGroup.Value;
                DebugFeatureEnableSwitchGroup.Value = DebugFeatureEnableSwitchGroup.Value;
            }
        }

        private List<Type> GetLogTypes()
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

            return allTypes;
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
            var allTyoes = GetLogTypes();
            return allTyoes.ConvertAll(item => item.FullName);
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

        public void SetDebugFeatureEnabled(string debugFeatureName, bool enabled)
        {
            DebugFeatureEnableSwitchGroup.Value.SetSwitchOn(debugFeatureName, enabled);
            DebugFeatureEnableSwitchGroup.Value = DebugFeatureEnableSwitchGroup.Value;
        }

        public void SetDebugFeatureEnabled(EDebugFeature debugFeature, bool enabled)
        {
            SetDebugFeatureEnabled(debugFeature.ToString(), enabled);
        }

        public bool IsDebugFeatureEnabled(EDebugFeature debugFeature)
        {
            bool ret = IsDebugFeatureEnabled(debugFeature.ToString());
            return ret;
        }

        public bool IsDebugFeatureEnabled(string debugFeatureName)
        {
            bool ret = DebugFeatureEnableSwitchGroup.Value.IsSwitchOn(debugFeatureName);
            return ret;
        }

        public List<string> GetAllDebugFeatureNames()
        {
            var allFeatures = new List<string>();
            foreach (EDebugFeature item in Enum.GetValues(typeof(EDebugFeature)))
                allFeatures.Add(item.ToString());

            return allFeatures;
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

    public class SwitchGroup
    {
        public List<SwitchInfo> switchInfos = new List<SwitchInfo>();

        public bool IsSwitchOn(string switchName)
        {
            var switchInfo = switchInfos.Find(item=>item.switchName == switchName);
            var ret = switchInfo != null && switchInfo.isOn;
            return ret;
        }

        public void SetSwitchOn(string switchName, bool isOn)
        {
            var switchInfo = switchInfos.Find(item => item.switchName == switchName);
            if (switchInfo != null)
                switchInfo.isOn = isOn;
        }
    }

    public class SwitchInfo
    {
        public string switchName;
        public bool isOn;
    }
}

