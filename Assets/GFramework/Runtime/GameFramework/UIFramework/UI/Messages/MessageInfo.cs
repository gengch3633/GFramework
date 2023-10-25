using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class MessageInfo
    {
        public bool isPureMessage;
        public string languageKey;
        public object[] parameters;
        public MessageInfo(string languageKey, params object[] parameters)
        {
            this.isPureMessage = false;
            this.languageKey = languageKey;
            this.parameters = parameters;
        }

        public MessageInfo(string languageKey, bool isPureMessage)
        {
            this.isPureMessage = isPureMessage;
            this.languageKey = languageKey;
        }

        public string GetFormatText(ILanguageSystem languageSystem)
        {
            var message = isPureMessage ? languageKey: languageSystem.FormatLanguageText(languageKey, parameters);
            return message;
        }
    }
}