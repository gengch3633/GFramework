using System;

namespace GameFramework
{
    public partial class SettingPopup : UIPopup
    {
        private int clickBackDoorCount = 0;
        private DateTime lastClickTime = DateTime.MinValue;

        private void OnClickBackDoor()
        {
            if (debugModel.IsDebugFeatureEnabled<Debug_BackDoor>())
            {
                uiSystem.OpenPopup<DebugPopup>();
                return;
            }

            var timeSpan = DateTime.Now - lastClickTime;
            lastClickTime = DateTime.Now;
            if (timeSpan.TotalSeconds < 0.3f)
            {
                clickBackDoorCount++;
                if (clickBackDoorCount >= 10)
                {
                    uiSystem.OpenPopup<DebugPopup>();
                    debugModel.SetDebugFeatureEnabled(typeof(Debug_BackDoor).FullName ,true);
                }
            }
            else
                clickBackDoorCount = 0;
        }
    }
}