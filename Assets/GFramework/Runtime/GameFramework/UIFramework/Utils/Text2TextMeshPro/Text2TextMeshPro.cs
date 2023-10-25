using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class Text2TextMeshPro : MonoBehaviour
    {
        protected virtual string GetFontAssetName()
        {
            return "Roboto-Bold SDF";
        }
        protected virtual string GetMaterialName()
        {
            return "COCOGOOSE_SDF_Material_BtnGreen";
        }

        protected void Reset()
        {
            var text = GetComponent<Text>();
            if(text != null)
            {
                var color = text.color;
                var fontSize = text.fontSize;
                var alignment = text.alignment;
                var textString = text.text;

                DestroyImmediate(text);
                var tmpText = gameObject.AddComponent<TextMeshProUGUI>();

                TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>($"Fonts & Materials/{GetFontAssetName()}");
                Material material = Resources.Load<Material>($"Fonts & Materials/{GetMaterialName()}");

                tmpText.font = fontAsset;
                tmpText.fontSharedMaterial = material;

                //tmpText.enableAutoSizing = true;
                tmpText.text = textString;
                tmpText.fontSizeMin = 0;
                tmpText.enableWordWrapping = true;
                tmpText.alignment = TextAlignmentOptions.Midline;
                tmpText.verticalAlignment = VerticalAlignmentOptions.Middle;
                tmpText.horizontalAlignment = HorizontalAlignmentOptions.Center;
                //if(alignment.ToString().Contains("Left"))
                //    tmpText.horizontalAlignment = HorizontalAlignmentOptions.Left;
                //if (alignment.ToString().Contains("Right"))
                //    tmpText.horizontalAlignment = HorizontalAlignmentOptions.Right;


                tmpText.fontSize = fontSize;
                tmpText.color = color;
            }
            
        }
    }

}

