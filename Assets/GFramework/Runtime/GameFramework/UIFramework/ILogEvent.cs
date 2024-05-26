using System.Collections.Generic;

namespace GameFramework
{
    public interface ILogEvent
    {
        void LogEvent(string eventName, Dictionary<string, object> paramDict);
    }
}

