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
        /// AppsFlyer的回调函数1
        /// </summary>
        /// <param name="normalPlayer">true为自然流量，非网赚用户</param>
        /// <param name="referUser">推荐用户的id</param>
        private static void AppsFlyerConversionDataCallBack(bool normalPlayer, string referUser)
        {
            Debug.Log($"AppsFlyerConversionDataCallBack ret:{normalPlayer},info:{referUser}");
            //LocalPlayer.Instance.SetOrganic(normalPlayer);
            //回调
            //OnRedeemValueChange?.Invoke();
        }

        /// <summary>
        /// 设置用户的邀请链接
        /// </summary>
        /// <param name="info"></param>
        private static void AppsFlyerLinkCallBack(string info)
        {
            //LocalPlayer.Instance.AppsFlyShareLink = info;
        }
    }
}

