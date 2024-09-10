using Cysharp.Threading.Tasks;
using Framework;
using Newtonsoft.Json;
using Solitaire;
using System;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

namespace GameFramework
{
    public partial interface IPurchaseSystem : ISystem
    {
        void CheckRestoreSubscription();
        void RestoreTransactions(Action<bool> onCompleted);
    }

    public partial class PurchaseSystem : AbstractSystem, IPurchaseSystem, IStoreListener, ITypeLog
    {
        private IAppleExtensions appleExtensions;
        private IGooglePlayStoreExtensions googlePlayStoreExtensions;
        private Dictionary<string, SubscriptionInfo> allSubscriptionInfo = new Dictionary<string, SubscriptionInfo>();
        public void RestoreTransactions(Action<bool> onCompleted)
        {
            uiSystem.OpenPopup<PurchaseLoadingPopup>();
            appleExtensions?.RestoreTransactions((result, msg) => {
                uiSystem.ClosePopup<PurchaseLoadingPopup>();
                onCompleted?.Invoke(result);
                if (result)
                    uiSystem.OpenMessage<NormalMessage>(new MessageInfo("message_restore_success"));
                else
                    uiSystem.OpenMessage<NormalMessage>(new MessageInfo("message_restore_failed"));
                if (IsTypeLogEnabled())
                    Debug.LogError($"==> [PurchaseSystem] [RestoreTransactions] Result: {result}, msg: {msg}");
            });

            googlePlayStoreExtensions?.RestoreTransactions((result, msg) => {
                uiSystem.ClosePopup<PurchaseLoadingPopup>();
                onCompleted?.Invoke(result);
                if (result)
                    uiSystem.OpenMessage<NormalMessage>(new MessageInfo("message_restore_success"));
                else
                    uiSystem.OpenMessage<NormalMessage>(new MessageInfo("message_restore_failed"));
                if (IsTypeLogEnabled())
                    Debug.LogError($"==> [PurchaseSystem] [RestoreTransactions] Result: {result}, msg: {msg}");
            });
        }

        public void CheckRestoreSubscription()
        {
            if (storeController != null)
            {
                Dictionary<string, string> introductoryInfoDict = appleExtensions.GetIntroductoryPriceDictionary();
                bool isSubscribed = false;
                foreach (var item in storeController.products.all)
                {
                    if (item.availableToPurchase)
                    {
                        if (item.definition.type == ProductType.Subscription)
                        {
                            if (item.receipt != null)
                            {
                                if (IsTypeLogEnabled()) Debug.LogError($"==> [PurchaseSystem] [CheckRestoreSubscription] 1: {JsonConvert.SerializeObject(item)}");
                                if (CheckIfProductIsAvailableForSubscriptionManager(item.receipt))
                                {
                                    var hasKey = (introductoryInfoDict != null) && introductoryInfoDict.ContainsKey(item.definition.storeSpecificId);
                                    var introductoryJson = hasKey ? introductoryInfoDict[item.definition.storeSpecificId] : null;
                                    SubscriptionManager p = new SubscriptionManager(item, introductoryJson);
                                    SubscriptionInfo info = p.getSubscriptionInfo();
                                    allSubscriptionInfo[info.getProductId()] = info;

                                    if (info.isCancelled() != Result.True)
                                    {
                                        if (info.isFreeTrial() == Result.True)
                                            isSubscribed = true;
                                        else if (info.isExpired() == Result.False)
                                            isSubscribed = true;
                                    }

                                    if (IsTypeLogEnabled())
                                        LogSubscriptionInfo(info);
                                }
                                else
                                {
                                    if (IsTypeLogEnabled())
                                        Debug.LogError("This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class.");
                                }
                            }
                            else
                            {
                                if (IsTypeLogEnabled())
                                    Debug.LogError("the product should have a valid receipt");
                            }

                            if (isSubscribed)
                                Debug.LogError($"==> [PurchaseSystem], isSubscribed: {isSubscribed}");
                        }
                    }
                }
            }
        }

        private void LogSubscriptionInfo(SubscriptionInfo info)
        {
            Debug.LogError($"==> [PurchaseSystem] getProductId: " + info.getProductId());
            Debug.LogError($"==> [PurchaseSystem] getPurchaseDate: " + info.getPurchaseDate());
            Debug.LogError($"==> [PurchaseSystem] getExpireDate: " + info.getExpireDate());
            Debug.LogError($"==> [PurchaseSystem] isSubscribed: " + info.isSubscribed());
            Debug.LogError($"==> [PurchaseSystem] isExpired: " + info.isExpired());
            Debug.LogError($"==> [PurchaseSystem] isCancelled: " + info.isCancelled());
            Debug.LogError($"==> [PurchaseSystem] isFreeTrial: " + info.isFreeTrial());
            Debug.LogError($"==> [PurchaseSystem] isAutoRenewing: " + info.isAutoRenewing());
            Debug.LogError($"==> [PurchaseSystem] getRemainingTime: " + info.getRemainingTime());
            Debug.LogError($"==> [PurchaseSystem] isIntroductoryPricePeriod: " + info.isIntroductoryPricePeriod());
            Debug.LogError($"==> [PurchaseSystem] getIntroductoryPrice: " + info.getIntroductoryPrice());
            Debug.LogError($"==> [PurchaseSystem] getIntroductoryPricePeriodCycles: " + info.getIntroductoryPricePeriodCycles());
        }

        private bool CheckIfProductIsAvailableForSubscriptionManager(string receipt)
        {
            var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
            if (!receipt_wrapper.ContainsKey("Store") || !receipt_wrapper.ContainsKey("Payload"))
            {
                if(IsTypeLogEnabled()) Debug.LogError("The product receipt does not contain enough information");
                return false;
            }
            var store = (string)receipt_wrapper["Store"];
            var payload = (string)receipt_wrapper["Payload"];

            if (payload != null)
            {
                switch (store)
                {
                    case GooglePlay.Name:
                        {
                            var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                            if (!payload_wrapper.ContainsKey("json"))
                            {
                                if (IsTypeLogEnabled()) Debug.LogError("The product receipt does not contain enough information, the 'json' field is missing");
                                return false;
                            }
                            var original_json_payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode((string)payload_wrapper["json"]);
                            if (original_json_payload_wrapper == null || original_json_payload_wrapper["purchaseState"].ToString() != "0")
                            {
                                if (IsTypeLogEnabled()) Debug.LogError("The product receipt does not contain enough information, the 'developerPayload' field is missing");
                                return false;
                            }

                            return true;
                        }
                    case AppleAppStore.Name:
                    case AmazonApps.Name:
                    case MacAppStore.Name:
                            return true;
                    default:
                            return false;
                }
            }
            return false;
        }
    }
}

