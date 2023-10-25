using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[Serializable]
public class AFData
{
	public string install_time;
	public string af_status;
	public bool is_first_launch;
	public string af_referrer_customer_id;
	public string af_referrer_uid;
	public string media_source;
}

public class AppsFlyerTrackerCallbacks : MonoBehaviour {
	Action<bool, string> receiveConversionDataCallBack;
	Action< string> inviteLinkCallBack;

	// Use this for initialization
	void Start () {
		printCallback("AppsFlyerTrackerCallbacks on Start.....");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SetReceiveCallBack(Action<bool, string> callBack )
	{
		receiveConversionDataCallBack = callBack;

	}

	public void SetInviteLinkCallBack(Action<string> callBack)
	{
		inviteLinkCallBack = callBack;

	}

	public void didReceiveConversionData(string conversionData) {
		printCallback ("AppsFlyerTrackerCallbacks:: got conversion data = " + conversionData);

		AFData data = JsonUtility.FromJson<AFData>(conversionData);
		string referUser = "";
		if (data.media_source == "af_app_invites" && data.af_referrer_customer_id != null)
		{
			referUser=data.af_referrer_customer_id;
		}

		receiveConversionDataCallBack?.Invoke(data.af_status == "Organic", referUser);
		
	}
	
	public void didReceiveConversionDataWithError(string error) {
		printCallback ("AppsFlyerTrackerCallbacks:: got conversion data error = " + error);
	}
	
	public void didFinishValidateReceipt(string validateResult) {
		printCallback ("AppsFlyerTrackerCallbacks:: got didFinishValidateReceipt  = " + validateResult);
		
	}
	
	public void didFinishValidateReceiptWithError (string error) {
		printCallback ("AppsFlyerTrackerCallbacks:: got idFinishValidateReceiptWithError error = " + error);
		
	}
	
	public void onAppOpenAttribution(string validateResult) {
		printCallback ("AppsFlyerTrackerCallbacks:: got onAppOpenAttribution  = " + validateResult);
		
	}
	
	public void onAppOpenAttributionFailure (string error) {
		printCallback ("AppsFlyerTrackerCallbacks:: got onAppOpenAttributionFailure error = " + error);
		
	}
	
	public void onInAppBillingSuccess () {
		printCallback ("AppsFlyerTrackerCallbacks:: got onInAppBillingSuccess succcess");
		
	}
	public void onInAppBillingFailure (string error) {
		printCallback ("AppsFlyerTrackerCallbacks:: got onInAppBillingFailure error = " + error);
		
	}

	public void onInviteLinkGenerated (string link) {
		printCallback("AppsFlyerTrackerCallbacks:: generated userInviteLink "+link);
		inviteLinkCallBack?.Invoke(link);
	}

	public void onOpenStoreLinkGenerated (string link) {
		printCallback("onOpenStoreLinkGenerated:: generated store link "+link);
		Application.OpenURL(link);
	}

	void printCallback(string str) {
		Debug.Log(str);
	}
}
