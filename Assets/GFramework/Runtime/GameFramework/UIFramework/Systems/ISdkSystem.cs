using Framework;
using UnityEngine;

namespace GameFramework
{
    public interface ISdkSystem : ISystem
    {

    }
    public partial class SdkSystem: AbstractSystem, ISdkSystem
    {
        protected override void OnInit()
        {
            base.OnInit();
            //Init();

            FacebookManager.Instance.Init();
            AppsFlyerManager.Instance.Init();
            AppsFlyerManager.Instance.SetCallBack(AppsFlyerConversionDataCallBack, AppsFlyerLinkCallBack);
        }

        /// <summary>
        /// AppsFlyer�Ļص�����1
        /// </summary>
        /// <param name="normalPlayer">trueΪ��Ȼ����������׬�û�</param>
        /// <param name="referUser">�Ƽ��û���id</param>
        private static void AppsFlyerConversionDataCallBack(bool normalPlayer, string referUser)
        {
            Debug.Log($"AppsFlyerConversionDataCallBack ret:{normalPlayer},info:{referUser}");
            //LocalPlayer.Instance.SetOrganic(normalPlayer);
            //�ص�
            //OnRedeemValueChange?.Invoke();
        }

        /// <summary>
        /// �����û�����������
        /// </summary>
        /// <param name="info"></param>
        private static void AppsFlyerLinkCallBack(string info)
        {
            //LocalPlayer.Instance.AppsFlyShareLink = info;
        }
    }
}

