using AppsFlyerSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{

    public class AppsFlyerManager : MonoSingleton<AppsFlyerManager>, IAppsFlyerConversionData, IAppsFlyerUserInvite
    {
        public bool InitSuccessFlag { get; private set; } = false;
        public static bool ConversionSuccessFlag { get; private set; } = false;

        public static string referUser = "";
        public static string mediaSource = "";
        public static string afStatus = "";
        public static bool isFirstLaunch = false;


        private Action<bool, string> receiveConversionDataCallBack;
        private Action<string> inviteLinkCallBack;


        public void Init()
        {
            if (!InitSuccessFlag)
            {
                // These fields are set from the editor so do not modify!
                //******************************//
                AppsFlyer.setIsDebug(false);
                AppsFlyer.initSDK(SDKConst.SdkConfigProduction.appsFlyerKey, SDKConst.SDK_APPSFLYER_IOS_APPID, this);
                //******************************//

                AppsFlyer.startSDK();
                AppsFlyer.setAppInviteOneLinkID(SDKConst.SDK_APPSFLYER_LINKID);

                InitSuccessFlag = true;
            }

        }

        public void GenerateInviteLink(string userName, string userID)
        {
            Dictionary<string, string> inviteDicionary = new Dictionary<string, string>();
            inviteDicionary.Add("channel", "channel_name");
            inviteDicionary.Add("referrersName", userName);
            inviteDicionary.Add("referrerUID", userID);
            AppsFlyer.generateUserInviteLink(inviteDicionary, this);
        }

        public void SetCallBack(Action<bool, string> receiveConversionDataCallBack, Action<string> inviteLinkCallBack)
        {
            //Debug.Log($"AppsFlyerSetCallBack SetCallBack");
            this.inviteLinkCallBack = inviteLinkCallBack;
            this.receiveConversionDataCallBack = receiveConversionDataCallBack;
            //onConversionDataSuccess("{\"install_time\":\"2021 - 01 - 11 07:31:21.818\",\"af_status\":\"Organic\",\"af_message\":\"organic install\",\"is_first_launch\":true}");
        }

        // Mark AppsFlyer CallBacks
        //01-11 15:31:22.913 15133 15205 I Unity   : AppsFlyer_Unity_v6.1.3 didReceiveConversionData called with 
        //{"install_time":"2021-01-11 07:31:21.818","af_status":"Organic","af_message":"organic install","is_first_launch":true}

        //onConversionDataSuccess data is null, conversionData:{"redirect_response_data":null,"adgroup_id":null,"engmnt_source":null,"retargeting_conversion_type":"none","orig_cost":"0.0","is_first_launch":false,"af_click_lookback":"7d","af_cpi":null,"iscache":true,"click_time":"2021-01-20 03:05:07.729","is_branded_link":null,"match_type":"probabilistic","adset":null,"campaign_id":null,"install_time":"2021-01-20 03:05:52.488","media_source":"appsflyer_sdk_test_int","agency":null,"clickid":"8ff7d8ab-021b-4310-8e19-0c6a8e2133a4","af_siteid":null,"af_status":"Non-organic","af_sub1":null,"cost_cents_USD":"0","af_sub5":null,"af_sub4":null,"af_sub3":null,"af_sub2":null,"adset_id":null,"esp_name":null,"campaign":"None","http_referrer":"http:\/\/sdktest.appsflyer.com\/sdk-integration-test\/install\/click-launch?appId=solitaire.collection.win.real&sid=8ff7d8ab-021b-4310-8e19-0c6a8e2133a4&platform=android&store=googlePlay&ts=1611111672&advertising_id=1ed6c2e1-6490-4c3a-887d-a2b33ca96f66","is_universal_link":null,"adgroup":null}
        //01-20 11:25:41.948  8623  8733 I Unity   : AppsFlyer_Unity_v6.1.3 didReceiveConversionData called with {"referrersName":"Guest-1235751","redirect_response_data":null,"adgroup_id":null,"engmnt_source":null,"retargeting_conversion_type":"none","orig_cost":"0.0","referrerUID":"1235751","is_first_launch":true,"af_click_lookback":"7d","af_cpi":null,"iscache":true,"click_time":"2021-01-20 03:25:02.143","is_branded_link":null,"match_type":"probabilistic","af_referrer_uid":"1611111949985-3186503044468121021","adset":null,"af_channel":"channel_name","campaign_id":null,"shortlink":"79c46c6","install_time":"2021-01-20 03:25:39.243","media_source":"af_app_invites","agency":null,"af_siteid":"solitaire.collection.win.real","af_status":"Non-organic","af_sub1":null,"cost_cents_USD":"0","af_sub5":null,"af_sub4":null,"af_sub3":null,"af_sub2":null,"adset_id":null,"esp_name":null,"campaign":"None","http_referrer":null,"is_universal_link":null,"adgroup":null}
        public void onConversionDataSuccess(string conversionData)
        {

            AppsFlyer.AFLog("didReceiveConversionData", conversionData);
            Dictionary<string, object> conversionDataDictionary
                = AppsFlyer.CallbackStringToDictionary(conversionData);
            // add deferred deeplink logic here



            if (conversionDataDictionary != null)
            {
                if (conversionDataDictionary.TryGetValue("media_source", out object mediaSourceObj))
                {
                    mediaSource = (string)mediaSourceObj;

                    if (mediaSource == "af_app_invites")
                    {
                        if (conversionDataDictionary.TryGetValue("referrerUID", out object referUserObj))
                        {
                            referUser = (string)referUserObj;
                        }
                    }
                }

                if (conversionDataDictionary.TryGetValue("af_status", out object afStatusObj))
                {
                    afStatus = (string)afStatusObj;
                }

                if (conversionDataDictionary.TryGetValue("is_first_launch", out object isFirstLaunchObj))
                {
                    isFirstLaunch = (bool)isFirstLaunchObj;
                }

                if (!string.IsNullOrEmpty(afStatus))
                {
                    //StaticModule.AppsFlyerInstallStatus(afStatus);


                    //StaticModule.AppsFlyerMediaSource(mediaSource);
                    //来自Google和Facebook广告的安装，也认为是正常用户
                    bool fromGoogleOrFacebook = mediaSource.ToLower().Contains("google") || mediaSource.ToLower().Contains("facebook");
                    bool normalPlayer = afStatus == "Organic" || fromGoogleOrFacebook;

                    receiveConversionDataCallBack?.Invoke(normalPlayer, referUser);

                    if (isFirstLaunch)
                    {
                        //StaticModule.AppsFlyerPlayerType(normalPlayer ? "normal" : "cash");
                    }
                }
            }
            else
            {
                Debug.Log($"onConversionDataSuccess conversionDataDictionary is null,conversionData:{conversionData}");
            }

            ConversionSuccessFlag = true;
        }

        /// <summary>
        /// 打印Fetch相关的信息
        /// </summary>
        /// <returns></returns>
        public static string StaticToString()
        {

            string ret = $"ConversionSuccessFlag:{ConversionSuccessFlag},\nreferUser:{referUser},\n" +
                $"mediaSource:{mediaSource},\nafStatus:{afStatus},\nisFirstLaunch:{isFirstLaunch}\n";
            return ret;
        }



        public void onConversionDataFail(string error)
        {
            AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
        }

        public void onAppOpenAttribution(string attributionData)
        {
            AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
            Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
            // add direct deeplink logic here
        }

        public void onAppOpenAttributionFailure(string error)
        {
            AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
        }

        public void onInviteLinkGenerated(string link)
        {
            AppsFlyer.AFLog("onInviteLinkGenerated", link);
            inviteLinkCallBack?.Invoke(link);
        }

        public void onInviteLinkGeneratedFailure(string error)
        {
            //throw new NotImplementedException();
            AppsFlyer.AFLog("onInviteLinkGeneratedFailure", error);
        }

        public void onOpenStoreLinkGenerated(string link)
        {
            //throw new NotImplementedException();
            AppsFlyer.AFLog("onOpenStoreLinkGenerated", link);
        }

        public void SendEvent(string eventName, Dictionary<string, string> eventValues = null)
        {
            AppsFlyer.sendEvent(eventName, eventValues);
        }
    }
}
