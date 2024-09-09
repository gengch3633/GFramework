using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace GameFramework
{
    public partial interface IEventSystem
    {
		void LogGirlSelectOpenEvent();
		void LogGirlSelectBtnDiamondEvent(bool diamond_enough);
		void LogGirlSelectSelectGameEvent(int diamond_count);
		void LogGirlSelectToggleSwitchEvent(string toggle_name);
		void LogGirlPlayOpenEvent();
		void LogGirlPlaySpine0Event(string spine_name);
		void LogGirlPlaySpine1Event(string spine_name);
		void LogGirlPlaySpine2Event(string spine_name);
		void LogGirlPlaySpine3Event(string spine_name);
		void LogGirlPlaySpine4Event(string spine_name);
		void LogGirlPlaySpine5Event(string spine_name);
		void LogGirlPlaySpine6Event(string spine_name);
		void LogGirlPlaySpine7Event(string spine_name);
		void LogGirlPlaySpine8Event(string spine_name);
		void LogGirlPlaySpine9Event(string spine_name);
		void LogGirlPlaySpine10Event(string spine_name);
		void LogGirlPlaySpine11Event(string spine_name);
		void LogGirlPlaySpine12Event(string spine_name);
		void LogGirlPlaySpine13Event(string spine_name);
		void LogGirlPlaySpine14Event(string spine_name);
		void LogGirlPlaySpine15Event(string spine_name);
		void LogGirlPlaySpine0WinEvent(string spine_name);
		void LogGirlPlaySpine1WinEvent(string spine_name);
		void LogGirlPlaySpine2WinEvent(string spine_name);
		void LogGirlPlaySpine3WinEvent(string spine_name);
		void LogGirlPlaySpine4WinEvent(string spine_name);
		void LogGirlPlaySpine5WinEvent(string spine_name);
		void LogGirlPlaySpine6WinEvent(string spine_name);
		void LogGirlPlaySpine7WinEvent(string spine_name);
		void LogGirlPlaySpine8WinEvent(string spine_name);
		void LogGirlPlaySpine9WinEvent(string spine_name);
		void LogGirlPlaySpine10WinEvent(string spine_name);
		void LogGirlPlaySpine11WinEvent(string spine_name);
		void LogGirlPlaySpine12WinEvent(string spine_name);
		void LogGirlPlaySpine13WinEvent(string spine_name);
		void LogGirlPlaySpine14WinEvent(string spine_name);
		void LogGirlPlaySpine15WinEvent(string spine_name);
		void LogGirlPlayCardEvent();
		void LogGirlPlayShootEvent();
		void LogGirlPlayHeartEvent();
		void LogGirlPlayDiceEvent();
		void LogGirlPlayBoxEvent();
		void LogGirlPlayCardFailedEvent(int failed_count);
		void LogGirlPlayShootFailedEvent(int failed_count);
		void LogGirlPlayHeartFailedEvent(int failed_count);
		void LogGirlPlayDiceFailedEvent(int failed_count);
		void LogGirlPlayBoxFailedEvent(int failed_count);
		void LogGirlPlayCardWinEvent(int failed_count);
		void LogGirlPlayShootWinEvent(int failed_count);
		void LogGirlPlayHeartWinEvent(int failed_count);
		void LogGirlPlayDiceWinEvent(int failed_count);
		void LogGirlPlayBoxWinEvent(int failed_count);
		void LogGirlLoseOpenEvent(string game_name);
		void LogGirlLoseClickLeaveEvent(string game_name);
		void LogGirlLoseClickDiamondEvent(string game_name, int diamond_count);
		void LogGirlLoseClickVideoEvent(string game_name);
		void LogGirlDiamondOpenEvent();
		void LogGirlUnlockedOpenEvent(string spine_index__spine_name);
	}

    public partial class EventSystem
    {
		public void LogGirlSelectOpenEvent()
		{
			var eventName = "girl_select_open";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_select_open_first")) LogGirlSelectOpenFirstEvent();
		}
		private void LogGirlSelectOpenFirstEvent()
		{
			var eventName = "girl_select_open_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		public void LogGirlSelectBtnDiamondEvent(bool diamond_enough)
		{
			var eventName = "girl_select_btn_diamond";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("diamond_enough", diamond_enough);
			LogEvent(eventName, paramDict);
		}
		public void LogGirlSelectSelectGameEvent(int diamond_count)
		{
			var eventName = "girl_select_select_game";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("diamond_count", diamond_count);
			LogEvent(eventName, paramDict);
		}
		public void LogGirlSelectToggleSwitchEvent(string toggle_name)
		{
			var eventName = "girl_select_toggle_switch";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("toggle_name", toggle_name);
			LogEvent(eventName, paramDict);
		}
		public void LogGirlPlayOpenEvent()
		{
			var eventName = "girl_play_open";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_open_first")) LogGirlPlayOpenFirstEvent();
		}
		private void LogGirlPlayOpenFirstEvent()
		{
			var eventName = "girl_play_open_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		public void LogGirlPlaySpine0Event(string spine_name)
		{
			var eventName = "girl_play_spine_0";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_0_first")) LogGirlPlaySpine0FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine1Event(string spine_name)
		{
			var eventName = "girl_play_spine_1";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_1_first")) LogGirlPlaySpine1FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine2Event(string spine_name)
		{
			var eventName = "girl_play_spine_2";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_2_first")) LogGirlPlaySpine2FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine3Event(string spine_name)
		{
			var eventName = "girl_play_spine_3";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_3_first")) LogGirlPlaySpine3FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine4Event(string spine_name)
		{
			var eventName = "girl_play_spine_4";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_4_first")) LogGirlPlaySpine4FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine5Event(string spine_name)
		{
			var eventName = "girl_play_spine_5";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_5_first")) LogGirlPlaySpine5FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine6Event(string spine_name)
		{
			var eventName = "girl_play_spine_6";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_6_first")) LogGirlPlaySpine6FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine7Event(string spine_name)
		{
			var eventName = "girl_play_spine_7";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_7_first")) LogGirlPlaySpine7FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine8Event(string spine_name)
		{
			var eventName = "girl_play_spine_8";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_8_first")) LogGirlPlaySpine8FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine9Event(string spine_name)
		{
			var eventName = "girl_play_spine_9";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_9_first")) LogGirlPlaySpine9FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine10Event(string spine_name)
		{
			var eventName = "girl_play_spine_10";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_10_first")) LogGirlPlaySpine10FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine11Event(string spine_name)
		{
			var eventName = "girl_play_spine_11";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_11_first")) LogGirlPlaySpine11FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine12Event(string spine_name)
		{
			var eventName = "girl_play_spine_12";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_12_first")) LogGirlPlaySpine12FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine13Event(string spine_name)
		{
			var eventName = "girl_play_spine_13";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_13_first")) LogGirlPlaySpine13FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine14Event(string spine_name)
		{
			var eventName = "girl_play_spine_14";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_14_first")) LogGirlPlaySpine14FirstEvent(spine_name);
		}
		public void LogGirlPlaySpine15Event(string spine_name)
		{
			var eventName = "girl_play_spine_15";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_15_first")) LogGirlPlaySpine15FirstEvent(spine_name);
		}
		private void LogGirlPlaySpine0FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_0_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine1FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_1_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine2FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_2_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine3FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_3_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine4FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_4_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine5FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_5_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine6FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_6_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine7FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_7_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine8FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_8_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine9FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_9_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine10FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_10_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine11FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_11_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine12FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_12_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine13FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_13_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine14FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_14_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine15FirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_15_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		public void LogGirlPlaySpine0WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_0_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_0_win_first")) LogGirlPlaySpine0WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine1WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_1_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_1_win_first")) LogGirlPlaySpine1WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine2WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_2_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_2_win_first")) LogGirlPlaySpine2WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine3WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_3_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_3_win_first")) LogGirlPlaySpine3WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine4WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_4_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_4_win_first")) LogGirlPlaySpine4WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine5WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_5_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_5_win_first")) LogGirlPlaySpine5WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine6WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_6_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_6_win_first")) LogGirlPlaySpine6WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine7WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_7_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_7_win_first")) LogGirlPlaySpine7WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine8WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_8_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_8_win_first")) LogGirlPlaySpine8WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine9WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_9_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_9_win_first")) LogGirlPlaySpine9WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine10WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_10_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_10_win_first")) LogGirlPlaySpine10WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine11WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_11_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_11_win_first")) LogGirlPlaySpine11WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine12WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_12_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_12_win_first")) LogGirlPlaySpine12WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine13WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_13_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_13_win_first")) LogGirlPlaySpine13WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine14WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_14_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_14_win_first")) LogGirlPlaySpine14WinFirstEvent(spine_name);
		}
		public void LogGirlPlaySpine15WinEvent(string spine_name)
		{
			var eventName = "girl_play_spine_15_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_spine_15_win_first")) LogGirlPlaySpine15WinFirstEvent(spine_name);
		}
		private void LogGirlPlaySpine0WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_0_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine1WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_1_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine2WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_2_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine3WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_3_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine4WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_4_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine5WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_5_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine6WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_6_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine7WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_7_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine8WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_8_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine9WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_9_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine10WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_10_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine11WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_11_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine12WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_12_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine13WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_13_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine14WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_14_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlaySpine15WinFirstEvent(string spine_name)
		{
			var eventName = "girl_play_spine_15_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_name", spine_name);
			LogEvent(eventName, paramDict);
		}
		public void LogGirlPlayCardEvent()
		{
			var eventName = "girl_play_card";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_card_first")) LogGirlPlayCardFirstEvent();
		}
		public void LogGirlPlayShootEvent()
		{
			var eventName = "girl_play_shoot";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_shoot_first")) LogGirlPlayShootFirstEvent();
		}
		public void LogGirlPlayHeartEvent()
		{
			var eventName = "girl_play_heart";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_heart_first")) LogGirlPlayHeartFirstEvent();
		}
		public void LogGirlPlayDiceEvent()
		{
			var eventName = "girl_play_dice";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_dice_first")) LogGirlPlayDiceFirstEvent();
		}
		public void LogGirlPlayBoxEvent()
		{
			var eventName = "girl_play_box";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_box_first")) LogGirlPlayBoxFirstEvent();
		}
		private void LogGirlPlayCardFirstEvent()
		{
			var eventName = "girl_play_card_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayShootFirstEvent()
		{
			var eventName = "girl_play_shoot_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayHeartFirstEvent()
		{
			var eventName = "girl_play_heart_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayDiceFirstEvent()
		{
			var eventName = "girl_play_dice_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayBoxFirstEvent()
		{
			var eventName = "girl_play_box_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		public void LogGirlPlayCardFailedEvent(int failed_count)
		{
			var eventName = "girl_play_card_failed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
			if (IsMatchEventCount("girl_play_card_failed_count", out int matchCount)) LogGirlPlayCardFailedCountEvent(matchCount);
		}
		public void LogGirlPlayShootFailedEvent(int failed_count)
		{
			var eventName = "girl_play_shoot_failed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
			if (IsMatchEventCount("girl_play_shoot_failed_count", out int matchCount)) LogGirlPlayShootFailedCountEvent(matchCount);
		}
		public void LogGirlPlayHeartFailedEvent(int failed_count)
		{
			var eventName = "girl_play_heart_failed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
			if (IsMatchEventCount("girl_play_heart_failed_count", out int matchCount)) LogGirlPlayHeartFailedCountEvent(matchCount);
		}
		public void LogGirlPlayDiceFailedEvent(int failed_count)
		{
			var eventName = "girl_play_dice_failed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
			if (IsMatchEventCount("girl_play_dice_failed_count", out int matchCount)) LogGirlPlayDiceFailedCountEvent(matchCount);
		}
		public void LogGirlPlayBoxFailedEvent(int failed_count)
		{
			var eventName = "girl_play_box_failed";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
			if (IsMatchEventCount("girl_play_box_failed_count", out int matchCount)) LogGirlPlayBoxFailedCountEvent(matchCount);
		}
		private void LogGirlPlayCardFailedCountEvent(int count)
		{
			var eventName = "girl_play_card_failed_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayShootFailedCountEvent(int count)
		{
			var eventName = "girl_play_shoot_failed_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayHeartFailedCountEvent(int count)
		{
			var eventName = "girl_play_heart_failed_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayDiceFailedCountEvent(int count)
		{
			var eventName = "girl_play_dice_failed_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayBoxFailedCountEvent(int count)
		{
			var eventName = "girl_play_box_failed_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		public void LogGirlPlayCardWinEvent(int failed_count)
		{
			var eventName = "girl_play_card_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_card_win_first")) LogGirlPlayCardWinFirstEvent(failed_count);
			if (IsMatchEventCount("girl_play_card_win_count", out int matchCount)) LogGirlPlayCardWinCountEvent(matchCount);
		}
		public void LogGirlPlayShootWinEvent(int failed_count)
		{
			var eventName = "girl_play_shoot_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_shoot_win_first")) LogGirlPlayShootWinFirstEvent(failed_count);
			if (IsMatchEventCount("girl_play_shoot_win_count", out int matchCount)) LogGirlPlayShootWinCountEvent(matchCount);
		}
		public void LogGirlPlayHeartWinEvent(int failed_count)
		{
			var eventName = "girl_play_heart_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_heart_win_first")) LogGirlPlayHeartWinFirstEvent(failed_count);
			if (IsMatchEventCount("girl_play_heart_win_count", out int matchCount)) LogGirlPlayHeartWinCountEvent(matchCount);
		}
		public void LogGirlPlayDiceWinEvent(int failed_count)
		{
			var eventName = "girl_play_dice_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_dice_win_first")) LogGirlPlayDiceWinFirstEvent(failed_count);
			if (IsMatchEventCount("girl_play_dice_win_count", out int matchCount)) LogGirlPlayDiceWinCountEvent(matchCount);
		}
		public void LogGirlPlayBoxWinEvent(int failed_count)
		{
			var eventName = "girl_play_box_win";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_play_box_win_first")) LogGirlPlayBoxWinFirstEvent(failed_count);
			if (IsMatchEventCount("girl_play_box_win_count", out int matchCount)) LogGirlPlayBoxWinCountEvent(matchCount);
		}
		private void LogGirlPlayCardWinCountEvent(int count)
		{
			var eventName = "girl_play_card_win_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayShootWinCountEvent(int count)
		{
			var eventName = "girl_play_shoot_win_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayHeartWinCountEvent(int count)
		{
			var eventName = "girl_play_heart_win_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayDiceWinCountEvent(int count)
		{
			var eventName = "girl_play_dice_win_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayBoxWinCountEvent(int count)
		{
			var eventName = "girl_play_box_win_count";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("count", count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayCardWinFirstEvent(int failed_count)
		{
			var eventName = "girl_play_card_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayShootWinFirstEvent(int failed_count)
		{
			var eventName = "girl_play_shoot_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayHeartWinFirstEvent(int failed_count)
		{
			var eventName = "girl_play_heart_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayDiceWinFirstEvent(int failed_count)
		{
			var eventName = "girl_play_dice_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
		}
		private void LogGirlPlayBoxWinFirstEvent(int failed_count)
		{
			var eventName = "girl_play_box_win_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("failed_count", failed_count);
			LogEvent(eventName, paramDict);
		}
		public void LogGirlLoseOpenEvent(string game_name)
		{
			var eventName = "girl_lose_open";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("game_name", game_name);
			LogEvent(eventName, paramDict);
		}
		public void LogGirlLoseClickLeaveEvent(string game_name)
		{
			var eventName = "girl_lose_click_leave";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("game_name", game_name);
			LogEvent(eventName, paramDict);
		}
		public void LogGirlLoseClickDiamondEvent(string game_name, int diamond_count)
		{
			var eventName = "girl_lose_click_diamond";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("game_name", game_name);
			paramDict.Add("diamond_count", diamond_count);
			LogEvent(eventName, paramDict);
		}
		public void LogGirlLoseClickVideoEvent(string game_name)
		{
			var eventName = "girl_lose_click_video";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("game_name", game_name);
			LogEvent(eventName, paramDict);
		}
		public void LogGirlDiamondOpenEvent()
		{
			var eventName = "girl_diamond_open";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_diamond_open_first")) LogGirlDiamondOpenFirstEvent();
		}
		private void LogGirlDiamondOpenFirstEvent()
		{
			var eventName = "girl_diamond_open_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			LogEvent(eventName, paramDict);
		}
		public void LogGirlUnlockedOpenEvent(string spine_index__spine_name)
		{
			var eventName = "girl_unlocked_open";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_index__spine_name", spine_index__spine_name);
			LogEvent(eventName, paramDict);
			if (!IsEventFirstFired("girl_unlocked_open_first")) LogGirlUnlockedOpenFirstEvent(spine_index__spine_name);
		}
		private void LogGirlUnlockedOpenFirstEvent(string spine_index__spine_name)
		{
			var eventName = "girl_unlocked_open_first";
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("spine_index__spine_name", spine_index__spine_name);
			LogEvent(eventName, paramDict);
		}
	}
}

