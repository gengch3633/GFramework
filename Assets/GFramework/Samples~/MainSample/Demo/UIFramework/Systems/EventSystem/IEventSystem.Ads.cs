using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace GameFramework
{
    public partial interface IEventSystem
    {
		void LogAdRewardShowEvent(string place);
		void LogAdRewardShowFailedEvent(string place);
		void LogAdRewardShowSuccessEvent(string place);
		void LogAdRewardLoadEvent(string place);
		void LogAdRewardLoadYesEvent(string place);
		void LogAdRewardLoadNoEvent(string place);
		void LogAdRewardLoadReplaceEvent(string place);
		void LogAdRewardLoadReplaceYesEvent(string place);
		void LogAdRewardLoadReplaceNoEvent(string place);
		void LogAdIntShowEvent(string place);
		void LogAdIntShowFailedEvent(string place);
		void LogAdIntShowSuccessEvent(string place);
		void LogAdBannerShowEvent(int refresh_interval);
		void LogAdBannerRefreshEvent(int refresh_interval);
		void LogAdBannerRefreshSuccessEvent(int refresh_interval);
		void LogAdBannerRefreshFailedEvent(int refresh_interval);
	}

    public partial class EventSystem
    {
		public void LogAdRewardShowEvent(string place)
		{
			var eventName = "ad_reward_show";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
			if (IsMatchEventCount("ad_reward_show_count", out int matchCount)) LogAdRewardShowCountEvent(matchCount);
		}
		private void LogAdRewardShowCountEvent(int count)
		{
			var eventName = "ad_reward_show_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardShowFailedEvent(string place)
		{
			var eventName = "ad_reward_show_failed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardShowSuccessEvent(string place)
		{
			var eventName = "ad_reward_show_success";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
			if (IsMatchEventCount("ad_reward_show_success_count", out int matchCount)) LogAdRewardShowSuccessCountEvent(matchCount);
		}
		private void LogAdRewardShowSuccessCountEvent(int count)
		{
			var eventName = "ad_reward_show_success_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardLoadEvent(string place)
		{
			var eventName = "ad_reward_load";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardLoadYesEvent(string place)
		{
			var eventName = "ad_reward_load_yes";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardLoadNoEvent(string place)
		{
			var eventName = "ad_reward_load_no";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardLoadReplaceEvent(string place)
		{
			var eventName = "ad_reward_load_replace";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardLoadReplaceYesEvent(string place)
		{
			var eventName = "ad_reward_load_replace_yes";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardLoadReplaceNoEvent(string place)
		{
			var eventName = "ad_reward_load_replace_no";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
		}
		public void LogAdIntShowEvent(string place)
		{
			var eventName = "ad_int_show";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
			if (IsMatchEventCount("ad_int_show_count", out int matchCount)) LogAdIntShowCountEvent(matchCount);
		}
		private void LogAdIntShowCountEvent(int count)
		{
			var eventName = "ad_int_show_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		public void LogAdIntShowFailedEvent(string place)
		{
			var eventName = "ad_int_show_failed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
		}
		public void LogAdIntShowSuccessEvent(string place)
		{
			var eventName = "ad_int_show_success";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("place", place);
			LogEvent(eventName, paramDict);
			if (IsMatchEventCount("ad_int_show_success_count", out int matchCount)) LogAdIntShowSuccessCountEvent(matchCount);
		}
		private void LogAdIntShowSuccessCountEvent(int count)
		{
			var eventName = "ad_int_show_success_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		public void LogAdBannerShowEvent(int refresh_interval)
		{
			var eventName = "ad_banner_show";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("refresh_interval", refresh_interval);
			LogEvent(eventName, paramDict);
			if (IsMatchEventCount("ad_banner_show_count", out int matchCount)) LogAdBannerShowCountEvent(matchCount);
		}
		private void LogAdBannerShowCountEvent(int count)
		{
			var eventName = "ad_banner_show_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		public void LogAdBannerRefreshEvent(int refresh_interval)
		{
			var eventName = "ad_banner_refresh";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("refresh_interval", refresh_interval);
			LogEvent(eventName, paramDict);
		}
		public void LogAdBannerRefreshSuccessEvent(int refresh_interval)
		{
			var eventName = "ad_banner_refresh_success";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("refresh_interval", refresh_interval);
			LogEvent(eventName, paramDict);
			if (IsMatchEventCount("ad_banner_refresh_success_count", out int matchCount)) LogAdBannerRefreshSuccessCountEvent(matchCount);
		}
		private void LogAdBannerRefreshSuccessCountEvent(int count)
		{
			var eventName = "ad_banner_refresh_success_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		public void LogAdBannerRefreshFailedEvent(int refresh_interval)
		{
			var eventName = "ad_banner_refresh_failed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("refresh_interval", refresh_interval);
			LogEvent(eventName, paramDict);
		}
	}
}

