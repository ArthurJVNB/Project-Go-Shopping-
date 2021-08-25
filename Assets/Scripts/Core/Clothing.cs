using System;
using SIM.Core;
using UnityEngine;

namespace SIM.Core
{
    public class Clothing : MonoBehaviour, IInteractable, IPurchasable
    {
        [SerializeField] float price = 100f;

        bool canEquip;
        bool canBuy;

        public void Interact()
        {
            if (canBuy) { ShowBuyWindow(); }
            else if (canEquip) { Equip(); }
        }

        private void Equip()
        {
            print("Equipping " + name);
        }

        private void ShowBuyWindow()
        {
            print("<WINDOW APPEARS> Do you want to buy " + name + "?");
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
