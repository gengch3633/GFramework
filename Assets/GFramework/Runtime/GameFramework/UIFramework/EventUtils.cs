using System.Collections.Generic;
using Framework;
using Newtonsoft.Json;
using UnityEngine;

namespace GameFramework
{
	public partial class EventUtils: Singleton<EventUtils>, ITypeLog
	{

		private static List<ILogEvent> platformLogEventHelpers = new List<ILogEvent>();
		private List<int> GetCountList()
		{
			List<int> countList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
			return countList;
		}
		public bool IsTypeLogEnabled()
		{
			var debugSystem = GameApp.Interface.GetModel<IDebugModel>();
			var ret = debugSystem.IsTypeLogEnabled(typeof(EventUtils).FullName);

			return ret;
		}

		public void AddLogEventHelper(ILogEvent logHelper)
        {
			platformLogEventHelpers.Add(logHelper);
		}

		private static void LogEvent(string eventName, Dictionary<string, object> paramDict)
		{
			if (Instance.IsTypeLogEnabled())
				Debug.LogWarning($"==> [EventUtils] [LogEvent]: {eventName},{JsonConvert.SerializeObject(paramDict, Formatting.Indented)}");
            for (int i = 0; i < platformLogEventHelpers.Count; i++)
            {
				var logEventHelper = platformLogEventHelpers[i];
				logEventHelper.LogEvent(eventName, paramDict);
			}
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
	}
}

