using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

namespace GameFramework
{

    public class FacebookManager : MonoSingleton<FacebookManager>
    {
        public void Init()
        {
           InitSdk();
        }

        public void LogPurchasedEvent(int numItems, string contentType, string contentId, string currency, double price)
        {
            var parameters = new Dictionary<string, object>();
            parameters[AppEventParameterName.NumItems] = numItems;
            parameters[AppEventParameterName.ContentType] = contentType;
            parameters[AppEventParameterName.ContentID] = contentId;
            parameters[AppEventParameterName.Currency] = currency;
            FB.LogPurchase((float)price, currency, parameters);
        }


        void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
                InitSdk();
        }

        private static void InitSdk()
        {
            if (FB.IsInitialized)
                FB.ActivateApp();
            else
                FB.Init(FB.ActivateApp);
        }
    }
}

