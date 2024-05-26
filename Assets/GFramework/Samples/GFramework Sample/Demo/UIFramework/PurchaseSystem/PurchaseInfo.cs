
using UnityEngine.Purchasing;

namespace GameFramework
{
    public class PurchaseInfo
    {
        public int itemId;
        public string productId;
        public ProductType productType;
        public string price;
        public int coinCount;
        public EPurchaseTag purchaseTag;
        public int extraDesc;

        public string GetPrice(IStoreController storeController)
        {
            try { price = storeController.products.WithID(productId).metadata.localizedPriceString; }
            finally { }
            return price;
        }
    }

    public enum EPurchaseTag
    {
        None,
        BestValue,
        MostPopular
    }
}
