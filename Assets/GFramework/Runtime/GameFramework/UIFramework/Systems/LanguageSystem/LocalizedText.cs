using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using System.Text.RegularExpressions;
using TMPro;

namespace GameFramework
{
    public class LocalizedText : MonoController
	{
		public string key;
		public LetterType cap;

		private void Start()
		{
			SetText();
			this.RegisterEvent<LanguageChangedEvent>(evt => SetText())
				.UnRegisterWhenGameObjectDestroyed(gameObject);
		}

		private string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			char[] array = s.ToCharArray();
			array[0] = char.ToUpper(array[0]);
			return new string(array);
		}

		private void SetText()
        {
            if (string.IsNullOrEmpty(key)) return;

            string text = languageSystem.GetLanguangeText(key);
            string pattern = @"{([0-9]*.?)}";
            MatchCollection results = Regex.Matches(text, pattern);
            if (results.Count > 0) return;

            text = GetText(text);
            Text textComponent = GetComponent<Text>();
            TextMeshProUGUI tmpText = GetComponent<TextMeshProUGUI>();
            if(textComponent != null) textComponent.text = text;
            if(tmpText != null) tmpText.text = text;
        }

        private string GetText(string text)
        {
            switch (cap)
            {
                case LetterType.LOWER:
                    text = text.ToLower();
                    break;
                case LetterType.UPPER:
                    text = text.ToUpper();
                    break;
                case LetterType.EVERY_WORD:
                    text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
                    break;
                case LetterType.FIRST_LETTER:
                    text = UppercaseFirst(text.ToLower());
                    break;
            }

            return text;
        }
    }
}
