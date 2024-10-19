using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class MessageInfo
    {
        public string languageKey;
        public object[] parameters;
        public MessageInfo(string languageKey, params object[] parameters)
        {
            this.languageKey = languageKey;
            this.parameters = parameters;
        }

        public string GetFormatText(ILanguageSystem languageSystem)
        {
            var message = languageSystem.GetFormatLanguageText(languageKey, parameters);
            return message;
        }
    }
}