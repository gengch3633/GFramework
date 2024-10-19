
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Purchasing;

namespace GameFramework
{
    public class PurchaseInfo
    {
        public int itemId;
        public string productId;
        public ProductType productType;
        public string price;
        public string itemCountArrayString;
        public string tagString;

        public List<int> GetItemCountList()
        {
            var intList = JsonConvert.DeserializeObject<List<int>>(itemCountArrayString);
            return intList;
        }

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
