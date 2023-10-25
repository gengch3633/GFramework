//#define USE_FIREBASE //gengch

#if USE_FIREBASE
using Firebase.Analytics;
#endif
using AppsFlyerSDK;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameFramework
{
	public class EventUtils:Framework.Singleton<EventUtils>, ITypeLog
	{
		private static List<int> GetGamePlayCountList()
		{
			List<int> countList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
			return countList;
		}
		public bool IsTypeLogEnabled()
		{
			var debugSystem = GameApp.Interface.GetSystem<IDebugSystem>();
			var ret = debugSystem.IsTypeLogEnabled(typeof(EventUtils).FullName);

			return ret;
		}
		private static void LogEvent(string eventName, Dictionary<string, object> paramDict)
		{
			LogFirebaseEvent(eventName, paramDict);
			LogAppsFlyerEvent(eventName, paramDict);
		}

		private static void LogFirebaseEvent(string stringName, Dictionary<string, object> dict)
		{
#if USE_FIREBASE
			var paramList = new List<Parameter>();
			foreach (KeyValuePair<string, object> kv in dict)
			{
				var typeName = kv.Value.GetType().Name;
				Parameter param = null;
				if (typeName == typeof(int).Name)
					param = new Parameter(kv.Key, (int)kv.Value);
				if (typeName == typeof(long).Name)
					param = new Parameter(kv.Key, (long)kv.Value);
				if (typeName == typeof(string).Name)
					param = new Parameter(kv.Key, (string)kv.Value);
				if (param != null)
					paramList.Add(param);
			}

			if (EventUtils.Instance.IsTypeLogEnabled()) Debug.LogError($"==> [EventUtils] LogFirebaseEvent 1: {stringName}, {JsonConvert.SerializeObject(dict)}");
			FirebaseAnalytics.LogEvent(stringName, paramList.ToArray());
#endif
		}

		private static void LogAppsFlyerEvent(string stringName, Dictionary<string, object> dict)
		{
			var stringDict = dict.ToDictionary(item => item.Key, item => item.Value.ToString());
			AppsFlyer.sendEvent(stringName, stringDict);
		}

		public static void LogAdRewardTrigerEvent(string pos)
		{
			var eventName = "ad_reward_triger";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("pos", pos);
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogAdRewardWatchCompletedEvent()
		{
			var eventName = "ad_reward_watch_completed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogAdRewardWatchCancelEvent()
		{
			var eventName = "ad_reward_watch_cancel";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogAdRewardLoadingEvent()
		{
			var eventName = "ad_reward_loading";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogAdRewardLoadingYesEvent()
		{
			var eventName = "ad_reward_loading_yes";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogAdRewardLoadingNoEvent()
		{
			var eventName = "ad_reward_loading_no";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogAdRewardLoadingReplaceEvent()
		{
			var eventName = "ad_reward_loading_replace";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogAdRewardLoadingReplaceNoEvent()
		{
			var eventName = "ad_reward_loading_replace_no";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogAdInterstitialTrigerEvent(string pos)
		{
			var eventName = "ad_interstitial_triger";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("pos", pos);
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogAdInterstitialNoEvent()
		{
			var eventName = "ad_interstitial_no";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogAdInterstitialYesEvent()
		{
			var eventName = "ad_interstitial_yes";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogLocalNofificationScheduledEvent()
		{
			var eventName = "Local_Nofification_Scheduled";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogLocalNofificationCancelEvent()
		{
			var eventName = "Local_Nofification_Cancel";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogLocalNofificationInvokeAppEvent()
		{
			var eventName = "Local_Nofification_Invoke_App";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogALoadingSceneEvent(string network)
		{
			var eventName = "a_loading_scene";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("network", network);
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogAGameSceneEvent()
		{
			var eventName = "a_game_scene";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogRoundOverEvent(int rounds)
		{
			var eventName = "round_over";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("rounds", rounds);
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogGamestartEvent(int gameid)
		{
			var eventName = "gamestart";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("gameid", gameid);
			EventUtils.LogEvent(eventName, paramDict);

			if (!GetGamePlayCountList().Contains(gameid))
				return;

			var eventName_2 = $"gamestart_{gameid:D3}";
			Dictionary<string, object> paramDict_2 = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName_2, paramDict_2);

		}
		public static void LogGameendEvent(int rank)
		{
			var eventName = "gameend";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("rank", rank);
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogViewRulesEvent()
		{
			var eventName = "view_rules";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogRestartEvent(int round)
		{
			var eventName = "restart";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("round", round);
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogScoreNextEvent()
		{
			var eventName = "score_next";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogScorePreEvent()
		{
			var eventName = "score_pre";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogClickTopsbEvent()
		{
			var eventName = "click_topsb";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			EventUtils.LogEvent(eventName, paramDict);
		}
		public static void LogStAvatarEvent(int avatar)
		{
			var eventName = "st_avatar";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("avatar", avatar);
			EventUtils.LogEvent(eventName, paramDict);
		}
	}
}

