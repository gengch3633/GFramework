using Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameFramework
{
    public interface IDebugSystem : ISystem
    {
        List<string> GetAllTypeLogNames();
        bool IsTypeLogEnabled(string typeFullName);
        void SetTypeLogEnabled(string typeFullName, bool enabled);

        List<string> GetAllDebugFeatureNames();
        void SetDebugFeatureEnabled(string debugFeatureName, bool enabled);
        bool IsDebugFeatureEnabled(string debugFeatureName);
        void SetDebugFeatureEnabled(EDebugFeature debugFeature, bool enabled);
        bool IsDebugFeatureEnabled(EDebugFeature debugFeature);
        void RecoverGameDataFromFile();
        void CopyGameData();
    }

    public class DebugSystem : AbstractSystem, IDebugSystem
    {
        public Dictionary<string, bool> typeLogEnableDict = new Dictionary<string, bool>();
        public Dictionary<string, bool> debugFeatureDict = new Dictionary<string, bool>();
        private DebugSystem debugRecord;

        protected override void OnInit()
        {
            base.OnInit();
            debugRecord = ReadInfoWithReturnNew<DebugSystem>();

            if(debugRecord.typeLogEnableDict.Count <= 0)
            {
                foreach (Type item in GetLogTypes())
                    debugRecord.typeLogEnableDict.Add(item.FullName, false);

                foreach (EDebugFeature item in System.Enum.GetValues(typeof(EDebugFeature)))
                    debugRecord.debugFeatureDict.Add(item.ToString(), false);
                SaveInfo(debugRecord);
            }

            //SetDebugFeatureEnabled(EDebugFeature.WatchRivalCard, true);
            //SetDebugFeatureEnabled(EDebugFeature.TestTutorial3, true);
        }

        private List<Type> GetLogTypes()
        {
            var currentNameSpaceTypes = Assembly.GetExecutingAssembly().GetTypes();
            var allTypes = new List<Type>();
            allTypes.AddRange(currentNameSpaceTypes);
            allTypes = allTypes.FindAll(item => item.FullName.StartsWith("GameFramework.") || item.FullName.StartsWith("Framework.") || item.FullName.StartsWith("Hearts."));
            allTypes = allTypes.FindAll(item => item.GetInterface(typeof(ITypeLog).Name) != null);

            var childClasses = new List<Type>();
            allTypes.ForEach(childClass =>
            {
                var parentClasses = allTypes.FindAll(item => childClass.IsSubclassOf(item));
                if (parentClasses.Count > 0)
                    childClasses.Add(childClass);
            });

            childClasses.ForEach(item => allTypes.Remove(item));

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
            debugRecord.typeLogEnableDict[typeFullName] = enabled;
            SaveInfo(debugRecord);
        }

        public bool IsTypeLogEnabled(string typeFullName)
        {
            var ret = debugRecord.typeLogEnableDict.ContainsKey(typeFullName) && debugRecord.typeLogEnableDict[typeFullName];
            return ret;
        }

        public void SetDebugFeatureEnabled(string debugFeatureName, bool enabled)
        {
            debugRecord.debugFeatureDict[debugFeatureName] = enabled;
            SaveInfo(debugRecord);
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
            bool ret = debugRecord.debugFeatureDict.ContainsKey(debugFeatureName) && debugRecord.debugFeatureDict[debugFeatureName];
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
}

