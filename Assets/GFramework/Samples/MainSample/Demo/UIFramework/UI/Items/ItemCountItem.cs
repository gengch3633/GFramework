using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework
{
    public class ItemCountItem : MonoController
    {
        public ItemType itemType;
        public Text itemCountTextVar;
        protected override void MonoStart()
        {
            base.MonoStart();
            if(itemType == ItemType.Coin)
                itemCountTextVar.text = $"{userModel.Coins.Value}";
            if (itemType == ItemType.Diamond)
                itemCountTextVar.text = $"{userModel.Diamonds.Value}";
            if (itemType == ItemType.Score)
                itemCountTextVar.text = $"{userModel.Score.Value}";
        }
    }
}

