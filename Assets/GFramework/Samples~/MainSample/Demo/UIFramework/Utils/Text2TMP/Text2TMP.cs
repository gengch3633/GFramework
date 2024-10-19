using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Text2TMP : MonoBehaviour
{
    private Dictionary<string, FontAssetGroup> fontAssetGroupDict = new Dictionary<string, FontAssetGroup>();

    [ValueDropdown("FontAssetNames")]
    [OnValueChanged("OnFontAssetNameChanged")]
    [PropertyOrder(2)]
    public string FontAssetName;
    private static IEnumerable FontAssetNames;

    [ValueDropdown("FontMaterialNames")]
    [OnValueChanged("OnFontMaterialNameChanged")]
    [PropertyOrder(3)]
    public string FontMaterialName;
    private static IEnumerable FontMaterialNames;

    private void OnFontAssetNameChanged()
    {
        var fontAssetGroup = fontAssetGroupDict[FontAssetName];
        FontMaterialNames = fontAssetGroup.materialNames;
        Debug.LogError("==> OnFontAssetNameChanged: " + fontAssetGroup.assetName);
    }

    private void OnFontMaterialNameChanged()
    {
        Debug.LogError("==> OnFontMaterialNameChanged: " + FontMaterialName);
        if (FontAssetName == "None")
            return;
        var text = gameObject.GetComponent<Text>();
        var tmpText = gameObject.GetComponent<TextMeshProUGUI>();
        if(tmpText != null)
        {
            SetFontAsset(tmpText);
            return;
        }
          
        if (text != null)
        {
            var color = text.color;
            var fontSize = text.fontSize;
            var textString = text.text;

            DestroyImmediate(text);
            tmpText = gameObject.AddComponent<TextMeshProUGUI>();
            SetFontAsset(tmpText);

            var rectTransform = GetComponent<RectTransform>();
            var height = (int)(fontSize * 1.3f);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);

            tmpText.text = textString;
            tmpText.color = color;

            tmpText.fontSize = fontSize;
            tmpText.fontSizeMin = 0;
            tmpText.fontSizeMax = fontSize;
            tmpText.enableWordWrapping = true;
            tmpText.overflowMode = TextOverflowModes.Truncate;
            tmpText.enableAutoSizing = true;

            //tmpText.alignment = TextAlignmentOptions.Midline;
            tmpText.verticalAlignment = VerticalAlignmentOptions.Middle;
            tmpText.horizontalAlignment = HorizontalAlignmentOptions.Center;
        }
    }

    private void SetFontAsset(TextMeshProUGUI tmpText)
    {
#if UNITY_EDITOR
        var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(GetAssetPath(FontAssetName));
        var material = AssetDatabase.LoadAssetAtPath<Material>(GetAssetPath(FontMaterialName));
        tmpText.font = fontAsset;
        tmpText.fontSharedMaterial = material;
#endif
    }

    [Button(ButtonSizes.Medium)]
    [PropertyOrder(1)]
    private void FindAssets()
    {
#if UNITY_EDITOR
        var guids = AssetDatabase.FindAssets(typeof(Text2TMP).ToString()).ToList();
        var text2TMPFolder = guids.ConvertAll(item => AssetDatabase.GUIDToAssetPath(item)).Find(item=> new DirectoryInfo(item).Exists);
        var tmpResourceFolder = $"{text2TMPFolder}/Resources/Fonts & Materials";
        var tmpResourceDirInfo = new DirectoryInfo(tmpResourceFolder);
        var fontAssets = tmpResourceDirInfo.GetFiles("*.asset").ToList();
        var fontMaterials = tmpResourceDirInfo.GetFiles("*.mat").ToList();
        fontAssetGroupDict.Clear();
        var item = new ValueDropdownList<string>();
        FontAssetNames = item;
        var baseAssetGroup = new FontAssetGroup();
        item.Add(baseAssetGroup.assetName, baseAssetGroup.assetName);
        fontAssetGroupDict.Add(baseAssetGroup.assetName, baseAssetGroup);
        for (int i = 0; i < fontAssets.Count; i++)
        {
            var fontAsset = fontAssets[i];
            var materialAssets = fontMaterials.FindAll(item => Path.GetFileNameWithoutExtension(item.Name).Contains(Path.GetFileNameWithoutExtension(fontAsset.Name)));
            var fontAssetGroup = new FontAssetGroup(tmpResourceFolder, fontAsset.Name, materialAssets.ConvertAll(item=> item.Name));
            item.Add(fontAssetGroup.assetName, fontAssetGroup.assetName);
            fontAssetGroupDict.Add(fontAssetGroup.assetName, fontAssetGroup);
        }
#endif
    }

    public string GetAssetPath(string assetName)
    {
        var fontAssetGroup = fontAssetGroupDict[FontAssetName];
        return $"{fontAssetGroup.assetFolder}/{assetName}";
    }
}
