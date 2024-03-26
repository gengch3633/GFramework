using Cysharp.Threading.Tasks;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class GFrameworkTest : MonoBehaviour
{
    public Button btnTest;
    public Image bundleImage;

    async void Start()
    {
        GFramework.GFramework.TestPrint();
        btnTest.onClick.AddListener(OnBtnTestClick);
    }

    private async void OnBtnTestClick()
    {
        await YooAssetsManager.Instance.InitAsync(true);
        var handle = YooAssets.LoadAssetAsync<Sprite>("2");
        await handle.ToUniTask(this);
        bundleImage.sprite = handle.AssetObject as Sprite;
    }
}
