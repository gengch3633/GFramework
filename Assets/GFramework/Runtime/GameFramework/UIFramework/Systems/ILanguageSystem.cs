using Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public interface ILanguageSystem : ISystem
    {
        string GetLanguangeText(string key);
        string FormatLanguageText(string key, params object[] parameters);
        void SetLanguageType(LanguageType languageType);

        LanguageType GetLanguageType();
    }

    public class LanguageSystem : AbstractSystem, ILanguageSystem, ITypeLog
    {
        private List<LanguageInfo> languageInfos;
        private static readonly string Key = "LANGUAGE_SYSTEM";
        private LanguageType languageType = LanguageType.English;

        protected override void OnInit()
        {
            var resourceSystem = this.GetSystem<IResourceSystem>();
            languageInfos = resourceSystem.GetConfigInfos<LanguageInfo>();

            var languageTypeString = PlayerPrefs.GetString(Key, "");
            languageTypeString = languageTypeString != "" ? languageTypeString : Application.systemLanguage.ToString();
            Enum.TryParse(languageTypeString, out languageType);
        }

        public bool IsTypeLogEnabled()
        {
            var debugSystem = this.GetSystem<IDebugSystem>();
            var ret = debugSystem.IsTypeLogEnabled(typeof(LanguageSystem).FullName);
            return ret;
        }

        public string FormatLanguageText(string languangeKey, params object[] parameters)
        {
            return string.Format(GetLanguangeText(languangeKey), parameters);
        }

        public string GetLanguangeText(string languangeKey)
        {
            var languageInfo = languageInfos.Find(item => item.Key == languangeKey);

            if(IsTypeLogEnabled()) Debug.LogError("==> [GetLanguangeText]: " + languangeKey);

            if (languageInfo == null)
            {
                Debug.LogError("==> Data No Set: " + languangeKey);
                return $"{languangeKey}";
            }
            return languageInfo.GetLanaugeText(languageType);
        }

        public void SetLanguageType(LanguageType languageType)
        {
            this.languageType = languageType;
            PlayerPrefs.SetString(Key, languageType.ToString());
        }

        public LanguageType GetLanguageType()
        {
            return languageType;
        }
    }

    public class LanguageInfo
    {
        public int id;
        public string Key;
        public string English;
        public string ChineseSimplified;
        public string ChineseTraditional;
        public string French;
        public string German;
        public string Korean;
        public string Portuguese;
        public string Russian;
        public string Spanish;
        public string Japanese;
        public string Indonesian;

        public string GetLanaugeText(LanguageType languageType)
        {
            switch (languageType)
            {
                case (LanguageType.English): return English != "None" ? English:$"{English}_{languageType.ToString().Substring(0, 1)}";
                case (LanguageType.ChineseSimplified): return ChineseSimplified != "None" ? ChineseSimplified : $"{English}_{languageType.ToString().Substring(0, 1)}";
                case (LanguageType.ChineseTraditional): return ChineseTraditional != "None" ? ChineseTraditional : $"{English}_{languageType.ToString().Substring(0, 1)}";
                case (LanguageType.French): return French != "None" ? French : $"{English}_{languageType.ToString().Substring(0, 1)}";
                case (LanguageType.German): return German != "None" ? German : $"{English}_{languageType.ToString().Substring(0, 1)}";
                case (LanguageType.Korean): return Korean != "None" ? Korean : $"{English}_{languageType.ToString().Substring(0, 1)}";
                case (LanguageType.Portuguese): return Portuguese != "None" ? Portuguese : $"{English}_{languageType.ToString().Substring(0, 1)}";
                case (LanguageType.Russian): return Russian != "None" ? Russian : $"{English}_{languageType.ToString().Substring(0, 1)}";
                case (LanguageType.Spanish): return Spanish != "None" ? Spanish : $"{English}_{languageType.ToString().Substring(0, 1)}";
                case (LanguageType.Japanese): return Japanese != "None" ? Japanese : $"{English}_{languageType.ToString().Substring(0, 1)}";
                case (LanguageType.Indonesian): return Indonesian != "None" ? Indonesian : $"{English}_{languageType.ToString().Substring(0, 1)}";

                default:return English;
            }
        }
    }

    public enum LanguageType
    {
        English,
        ChineseSimplified,
        ChineseTraditional,
        French,
        German,
        Korean,
        Portuguese,
        Russian,
        Spanish,
        Japanese,
        Indonesian
    }
}

    

