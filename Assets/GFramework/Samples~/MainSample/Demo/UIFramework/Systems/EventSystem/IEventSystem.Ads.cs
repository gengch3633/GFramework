using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace GameFramework
{
    public partial interface IEventSystem
    {
        void LogAdRewardShowEvent();
        void LogAdRewardShowFailedEvent();
        void LogAdRewardShowSuccessEvent();
        void LogAdRewardLoadEvent();
        void LogAdRewardLoadYesEvent();
        void LogAdRewardLoadNoEvent();
        void LogAdIntShowEvent();
        void LogAdIntShowFailedEvent();
        void LogAdIntShowSuccessEvent();
        void LogAdBannerShowEvent(int refresh_interval);
        void LogAdRewardRefreshEvent(int refresh_interval);
        void LogAdRewardRefreshSuccessEvent(int refresh_interval);
        void LogAdRewardRefreshFailedEvent(int refresh_interval);
    }

    public partial class EventSystem
    {
		public void LogAdRewardShowEvent()
		{
			var eventName = "ad_reward_show";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
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
		public void LogAdRewardShowFailedEvent()
		{
			var eventName = "ad_reward_show_failed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardShowSuccessEvent()
		{
			var eventName = "ad_reward_show_success";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
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
		public void LogAdRewardLoadEvent()
		{
			var eventName = "ad_reward_load";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardLoadYesEvent()
		{
			var eventName = "ad_reward_load_yes";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardLoadNoEvent()
		{
			var eventName = "ad_reward_load_no";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		public void LogAdIntShowEvent()
		{
			var eventName = "ad_int_show";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
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
		public void LogAdIntShowFailedEvent()
		{
			var eventName = "ad_int_show_failed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		public void LogAdIntShowSuccessEvent()
		{
			var eventName = "ad_int_show_success";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
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
		}
		public void LogAdRewardRefreshEvent(int refresh_interval)
		{
			var eventName = "ad_reward_refresh";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("refresh_interval", refresh_interval);
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardRefreshSuccessEvent(int refresh_interval)
		{
			var eventName = "ad_reward_refresh_success";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("refresh_interval", refresh_interval);
			LogEvent(eventName, paramDict);
		}
		public void LogAdRewardRefreshFailedEvent(int refresh_interval)
		{
			var eventName = "ad_reward_refresh_failed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("refresh_interval", refresh_interval);
			LogEvent(eventName, paramDict);
		}
	}
}

