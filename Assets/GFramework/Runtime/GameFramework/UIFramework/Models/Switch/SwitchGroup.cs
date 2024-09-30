using System.Collections.Generic;

namespace GameFramework
{
    public class SwitchGroup
    {
        public List<SwitchInfo> switchInfos = new List<SwitchInfo>();

        public bool IsSwitchOn(string switchName)
        {
            var switchInfo = switchInfos.Find(item=>item.switchName == switchName);
            var ret = switchInfo != null && switchInfo.isOn;
            return ret;
        }

        public void SetSwitchOn(string switchName, bool isOn)
        {
            var switchInfo = switchInfos.Find(item => item.switchName == switchName);
            if (switchInfo != null)
                switchInfo.isOn = isOn;
            else
            {
                var info = new SwitchInfo() { switchName = switchName, isOn = isOn };
                switchInfos.Add(info);
            }
        }
    }
}

