using System.Collections;
using System.Collections.Generic;
using Framework;
using Newtonsoft.Json;
using Solitaire;
using UnityEngine;

namespace GameFramework
{
    public interface IEventTracker
    {
        void LogEvent(string eventName, Dictionary<string, object> paramDict);
    }
    public partial interface IEventSystem : ISystem, IEventTracker
    {
        void AddEventTracker(IEventTracker eventTracker);
        bool IsEventFirstFired(string eventName);
        bool IsMatchEventCount(string eventName, out int matchCount);
        void IncreaseEventFireCount(string eventName);
        int GetEventFireCount(string eventName);

        BindableProperty<List<string>> EventFirstFiredList { get; }
        BindableProperty<EventCountGroup> EventCountInfoGroup { get; }
    }

    public partial class EventSystem : AbstractSystem, IEventSystem, ITypeLog
    {
        private List<IEventTracker> eventTrackerList = new List<IEventTracker>();
        private List<int> matchCountList = new List<int>() { 1, 3, 6, 10, 30, 60, 100, 300, 600, 1000 };
        public BindableProperty<List<string>> EventFirstFiredList { get; } = new BindableProperty<List<string>>() { Value = new List<string>() };
        public BindableProperty<EventCountGroup> EventCountInfoGroup { get; } = new BindableProperty<EventCountGroup>() { Value = new EventCountGroup() };

        protected override void OnInit()
        {
            base.OnInit();
            var eventSystem = ReadInfoWithReturnNew<EventSystem>();
            CopyBindableClass(this, eventSystem, () => SaveInfo(this));
        }

        public void LogEvent(string eventName, Dictionary<string, object> paramDict)
        {
            if (IsTypeLogEnabled())
                Debug.Log($"==> [EventSystem] [LogEvent]: {eventName}, {JsonConvert.SerializeObject(paramDict)}");
            eventTrackerList.ForEach(item => item.LogEvent(eventName, paramDict));
        }
        public void AddEventTracker(IEventTracker eventTracker)
        {
            eventTrackerList.Add(eventTracker);
        }

        public int GetEventFireCount(string eventName)
        {
            return EventCountInfoGroup.Value.GetEventCount(eventName, SaveInfo);
        }

        public override void SaveInfo()
        {
            base.SaveInfo();
            SaveInfo(this);
        }

        public void IncreaseEventFireCount(string eventName)
        {
            EventCountInfoGroup.Value.IncreaseEventCount(eventName, SaveInfo);
        }

        public bool IsEventFirstFired(string eventName)
        {
            var isFired = EventFirstFiredList.Value.Contains(eventName);
            if(!isFired)
            {
                EventFirstFiredList.Value.Add(eventName);
                SaveInfo(this);
            }
            return isFired;
        }

        public bool IsMatchEventCount(string eventName, out int matchCount)
        {
            IncreaseEventFireCount(eventName);
            var fireCount = GetEventFireCount(eventName);
            var isMatch = matchCountList.Contains(fireCount);
            matchCount = fireCount;
            return isMatch;
        }

        public bool IsTypeLogEnabled()
        {
            var debugModel = this.GetModel<IDebugModel>();
            var ret = debugModel.IsTypeLogEnabled(this);
            return ret;
        }
    }
}

