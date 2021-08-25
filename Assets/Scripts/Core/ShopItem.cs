using System;
using SIM.Core;
using UnityEngine;

namespace SIM.Core
{
    public class ShopItem : MonoBehaviour, IInteractable, IPurchasable
    {
        [SerializeField] bool isForSale = true;
        [SerializeField] float price = 100f;

        public void Interact(out GameObject interactedGameObject)
        {
            interactedGameObject = null;

            if (isForSale)
            {
                interactedGameObject = gameObject;
                ShowBuyWindow();
            }
        }

        private void ShowBuyWindow()
        {
            print("<WINDOW APPEARS> Do you want to buy " + name + " for $" + price + "?");
        }

        public float GetPrice()
        {
            return price;
        }

        public bool TryToBuy(float buyersMoney, out GameObject boughtObject)
        {
            if (buyersMoney == price)
            {
                boughtObject = this.gameObject;
                return true;
            }

            boughtObject = null;
            return false;
        }
    }
}
