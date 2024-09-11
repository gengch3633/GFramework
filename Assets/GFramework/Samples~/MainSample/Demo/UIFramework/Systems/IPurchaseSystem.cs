using Cysharp.Threading.Tasks;
using Framework;
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
        string GetPrice(PurchaseInfo purchaseInfo);
        List<PurchaseInfo> GetPuchaseInfos();
        void Purchase(string productId, Action<bool, string> onCompleted);
    }

    public partial class PurchaseSystem : AbstractSystem, IPurchaseSystem, IStoreListener, ITypeLog
    {
        private string environment = "production";
        private IStoreController storeController;
        private IExtensionProvider extensionProvider;
        private Action<Product, bool, string> purchaseCallback;
        private IUISystem uiSystem;

        protected override void OnInit()
        {
            base.OnInit();
            uiSystem = this.GetSystem<IUISystem>();
            InitAsync().Forget();
        }

        public bool IsTypeLogEnabled()
        {
            var debugSystem = this.GetModel<IDebugModel>();
            var ret = debugSystem.IsTypeLogEnabled(this);
            return ret;
        }

        public async UniTask InitAsync()
        {
            var options = new InitializationOptions().SetEnvironmentName(environment);
            await UnityServices.InitializeAsync(options);
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            var resourceSystem = this.GetSystem<IResourceSystem>();
            var purchaseInfos = GameUtils.GetConfigInfos<PurchaseInfo>();
            purchaseInfos.ForEach(item => builder.AddProduct(item.productId, item.productType));
            UnityPurchasing.Initialize(this, builder);
        }

        public void Purchase(string productId, Action<bool,string> onCompleted)
        {
            GameUtils.Log(this, $"Purchase 11: {productId}");
            uiSystem.OpenPopup<PurchaseLoadingPopup>();
            Purchase(productId, (product, isSuccess, msg) => { 
                onCompleted.Invoke(isSuccess, msg);
                uiSystem.ClosePopup<PurchaseLoadingPopup>();
            });
        }

        private void Purchase(string productId, Action<Product, bool, string> callBack)
        {
            purchaseCallback = callBack;
            if (storeController != null)
            {
                GameUtils.Log(this, $"Purchase 21: {productId}");
                storeController.InitiatePurchase(productId);
            }
            else
            {
                GameUtils.Log(this, $"Purchase 22: {productId}");
                uiSystem.OpenMessage<NormalMessage>(new MessageInfo("message_network_error"));
                InitAsync().Forget();
                purchaseCallback?.Invoke(null, false, "Please check the network and try again");
            }
        }

        public List<PurchaseInfo> GetPuchaseInfos()
        {
            var purchaseInfos = GameUtils.GetConfigInfos<PurchaseInfo>();
            return purchaseInfos;
        }

        public string GetPrice(PurchaseInfo purchaseInfo)
        {
            return purchaseInfo.GetPrice(storeController);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            GameUtils.Log(this, $"1");
            this.storeController = controller;
            this.extensionProvider = extensions;
            this.appleExtensions = extensionProvider.GetExtension<IAppleExtensions>();
            this.googlePlayStoreExtensions = extensionProvider.GetExtension<IGooglePlayStoreExtensions>();

            // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
            // On non-Apple platforms this will have no effect; OnDeferred will never be called.
            GameUtils.Log(this, $"2");
            appleExtensions.RegisterPurchaseDeferredListener((item)=> {
                GameUtils.Log(this, "[OnInitialized] Purchase deferred: " + item.definition.id);
            });

            GameUtils.Log(this, $"3");
            CheckRestoreSubscription();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            GameUtils.Log(this, $"error: {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            GameUtils.Log(this, $"error: {error}, message: {message}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            GameUtils.Log(this, $"product: {product.definition.id}, failureReason: {failureReason}");
            purchaseCallback?.Invoke(product, false, $"{failureReason}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            GameUtils.Log(this, $"purchaseEvent��{purchaseEvent.purchasedProduct.definition.id}");
            var product = purchaseEvent.purchasedProduct;
            bool validPurchase = ValidateReceipt(product.receipt);
            purchaseCallback?.Invoke(product, validPurchase, "ProcessPurchase");

            return PurchaseProcessingResult.Complete;
        }

        private bool ValidateReceipt(string receipt)
        {
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            bool validPurchase = true; // Presume valid for platforms with no R.V.
#if UNITY_EDITOR
            bool isEditor = true;
#else
            bool isEditor = false;
#endif

#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
            // Unity IAP's validation logic is only included on these platforms.
            // Prepare the validator with the secrets we prepared in the Editor
            // obfuscation window.
            if (!isEditor)
            {
                try
                {
                    var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
                    // On Google Play, result has a single product ID.
                    // On Apple stores, receipts contain multiple products.
                    var result = validator.Validate(receipt);
                    // For informational purposes, we list the receipt(s)
                    if(IsTypeLogEnabled()) Debug.Log("Receipt is valid. Contents:");
                    foreach (IPurchaseReceipt productReceipt in result)
                    {
                        GameUtils.Log(this, $"productID: {productReceipt.productID}, purchaseDate: {productReceipt.purchaseDate}, transactionID: {productReceipt.transactionID}");
                    }
                }
                catch (IAPSecurityException)
                {
                    GameUtils.Log(this, $"ValidateReceipt 1, [Invalid receipt, not unlocking content]");
                    validPurchase = false;
                }
            }
#endif
            GameUtils.Log(this, $"ValidateReceipt 2, validPurchase��{validPurchase}");
            return validPurchase;
        }
    }
}

