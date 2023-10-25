using UnityEngine;

namespace GameFramework
{
    public class TMP_BtnGrey : Text2TextMeshPro
    {
        protected override string GetMaterialName()
        {
            var fontAssetName = GetFontAssetName();
            var fontName = GetType().Name;
            var materialName = $"{fontAssetName} Material_{fontName}";

            Debug.LogError("==> GetMaterialName: " + materialName);
            return materialName;
        }
    }

}

