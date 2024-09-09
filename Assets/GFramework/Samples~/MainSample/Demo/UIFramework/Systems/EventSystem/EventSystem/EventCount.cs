using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace Solitaire
{
    public class EventCountInfo
    {
        public string eventName;
        public int eventCount;
    }

    public class EventCountGroup
    {
        public List<EventCountInfo> eventCountInfos = new List<EventCountInfo>();

        public void IncreaseEventCount(string eventName, Action callback)
        {
            var switchInfo = eventCountInfos.Find(item => item.eventName == eventName);
            if (switchInfo == null)
            {
                switchInfo = new EventCountInfo() { eventName = eventName, eventCount = 0 };
                eventCountInfos.Add(switchInfo);
            }
            switchInfo.eventCount += 1;
            callback.Invoke();
        }

        public int GetEventCount(string eventName, Action callback)
        {
            var switchInfo = eventCountInfos.Find(item => item.eventName == eventName);
            if (switchInfo == null)
            {
                switchInfo = new EventCountInfo() { eventName = eventName, eventCount = 0 };
                eventCountInfos.Add(switchInfo);
            }
            callback.Invoke();
            return switchInfo.eventCount;
        }
    }
}

