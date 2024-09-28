using System.Collections.Generic;

public class FontAssetGroup
{
    public string assetFolder = "None";
    public string assetName = "None";
    public List<string> materialNames = new List<string>();

    public FontAssetGroup()
    {
    }

    public FontAssetGroup(string assetFolder, string assetName, List<string> materialNames)
    {
        this.assetFolder = assetFolder;
        this.assetName = assetName;
        this.materialNames = materialNames;
    }
}
