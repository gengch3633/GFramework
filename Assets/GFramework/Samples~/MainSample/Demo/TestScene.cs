using Cysharp.Threading.Tasks;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class TestScene : MonoBehaviour
{
    public Button btnTest;
    public Image bundleImage;

     void Start()
    {
        GFramework.GFramework.TestPrint();
        btnTest.onClick.AddListener(OnBtnTestClick);
    }

    private void OnBtnTestClick()
    {
        
    }
}
