using System;

namespace GameFramework
{
    public partial class SettingsPopup : UIPopup
    {
        private int clickBackDoorCount = 0;
        private DateTime lastClickTime = DateTime.MinValue;

        private void OnClickBackDoor()
        {
            if (debugSystem.IsDebugFeatureEnabled(EDebugFeature.OpenBackDoor))
            {
                uiSystem.OpenPopup(PopupType.DebugPopup);
                return;
            }

            var timeSpan = DateTime.Now - lastClickTime;
            lastClickTime = DateTime.Now;
            if (timeSpan.TotalSeconds < 0.3f)
            {
                clickBackDoorCount++;
                if (clickBackDoorCount >= 10)
                {
                    uiSystem.OpenPopup(PopupType.DebugPopup);
                    debugSystem.SetDebugFeatureEnabled(EDebugFeature.OpenBackDoor, true);
                }
            }
            else
                clickBackDoorCount = 0;
        }
    }
}