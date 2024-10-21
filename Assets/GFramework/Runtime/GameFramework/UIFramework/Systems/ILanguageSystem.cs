using Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public interface ILanguageSystem : ISystem
    {
        string GetLanguageText(LanguageType languageType, string languageKey = "setting_language_country");
        string GetLanguangeText(string key);
        string GetFormatLanguageText(string key, params object[] parameters);
        void SetLanguageType(LanguageType languageType);
        void SetLanguageText(Text t, string key);
        void SetLanguageText(TextMeshProUGUI t, string key);
        void SetFormatLanguageText(Text t, string key, params object[] parameters);
        void SetFormatLanguageText(TextMeshProUGUI t, string key, params object[] parameters);

        LanguageType GetLanguageType();
    }

    public class LanguageSystem : AbstractSystem, ILanguageSystem, ITypeLog
    {
        private List<LanguageInfo> languageInfos = new List<LanguageInfo>();
        private static string Key = "LanguageType";
        private LanguageType languageType = LanguageType.English;

        protected override void OnInit()
        {
            var normalConfig = GameUtils.GetConfigInfo<NormalConfig>();
            for (int i = 0; i < normalConfig.languageInfoKeys.Count; i++)
            {
                var languageInfoKey = normalConfig.languageInfoKeys[i];
                var suffix = languageInfoKey.Replace("LanguageInfo_", "");
                suffix = suffix.Replace("LanguageInfo", "");
                languageInfos.AddRange(GameUtils.GetConfigInfos<LanguageInfo>(suffix));
            }

            GameUtils.Log(this, $"languageInfos.Count: {languageInfos.Count}");
            var languageTypeString = PlayerPrefs.GetString(Key, "");
            languageTypeString = languageTypeString != "" ? languageTypeString : Application.systemLanguage.ToString();
            Enum.TryParse(languageTypeString, out languageType);
        }

        public string GetLanguageText(LanguageType languageType, string languageKey = "setting_language_country")
        {
            var languageInfo = languageInfos.Find(item => item.Key == languageKey);
            var props = languageInfo.GetType().GetFields();
            var languageDict = new Dictionary<string, string>();
            foreach (var item in props)
            {
                var key = item.Name;
                var value = languageInfo.GetType().GetProperty(key).GetValue(languageInfo, null).ToString();
                languageDict.Add(key, value);
            }
            var valueString = languageDict.ContainsKey(languageType.ToString()) ? languageDict[languageType.ToString()]: languageKey;
            GameUtils.Log(this, $"languangeKey: {languageKey}, valueString: {valueString}, languageDict: {JsonConvert.SerializeObject(languageDict)}");
            return valueString;
        }
        public void SetFormatLanguageText(Text t, string key, params object[] parameters)
        {
            t.text = GetFormatLanguageText(key, parameters);
        }

        public void SetLanguageText(Text t, string key)
        {
            t.text = GetFormatLanguageText(key);
        }

        public void SetFormatLanguageText(TextMeshProUGUI t, string key, params object[] parameters)
        {
            t.text = GetFormatLanguageText(key, parameters);
        }

        public void SetLanguageText(TextMeshProUGUI t, string key)
        {
            t.text = GetFormatLanguageText(key);
        }

        public bool IsTypeLogEnabled()
        {
            return GameUtils.IsTypeLogEnabled(this);
        }

        public string GetFormatLanguageText(string languangeKey, params object[] parameters)
        {
            return string.Format(GetLanguangeText(languangeKey), parameters);
        }

        public string GetLanguangeText(string languangeKey)
        {
            var languageInfo = languageInfos.Find(item => item.Key == languangeKey);
            GameUtils.Log(this, $"languangeKey: {languangeKey}");
            if (languageInfo == null)
            {
                GameUtils.Log(this, $"Data No Set: {languangeKey}");
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
                case (LanguageType.English): return English != "None" ? English : $"{English}_{languageType.ToString().Substring(0, 1)}";
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

                default: return English;
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



