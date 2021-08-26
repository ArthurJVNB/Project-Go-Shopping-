using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SIM.Core
{
    public class Item : MonoBehaviour, IInteractable, ITradable
    {
        public Action<Trader, Trader> onChangedOwner; // old, new

        public bool IsStackable { get { return isStackable; } }
        public Sprite UIImage { get { return uiImage; } }

        [SerializeField] bool isStackable;
        [SerializeField] Sprite uiImage;
        [SerializeField] bool isForSale = true;
        [SerializeField] float price = 100f;

        public Trader Owner { get; private set; }

        public void Interact(GameObject whoInteracts, out GameObject interactedGameObject)
        {
            interactedGameObject = null;

            if (isForSale && whoInteracts.CompareTag("Player"))
            {
                interactedGameObject = gameObject;
                ShowBuyWindow();
            }
        }

        private void ShowBuyWindow()
        {
            print("<WINDOW APPEARS> Do you want to buy " + name + " for $" + price + "?");
        }

        public Trader GetOwner()
        {
            return Owner;
        }

        public void SetOwner(Trader newOwner)
        {
            Trader oldOwner = Owner;
            Owner = newOwner;
            onChangedOwner?.Invoke(oldOwner, newOwner);
        }

        public float GetPrice()
        {
            return price;
        }

        public bool TryToTrade(Trader buyer, out Item boughtItem)
        {
            boughtItem = null;
            bool result = false;

            if (!isForSale)
            {
                result = false;
            }
            else if (Owner)
            {
                result = Owner.Trade(this, buyer);
                if (result) boughtItem = this;
            }
            else
            {
                Inventory buyersInventory = buyer.GetComponent<Inventory>();
                result = buyersInventory.SubtractMoney(price);
                if (result) boughtItem = this;
            }

            return result;

            // Inventory ownersInventory = null;
            // if (Owner) { ownersInventory = Owner.GetComponent<Inventory>(); }
            // Inventory buyersInventory = buyer.GetComponent<Inventory>();

            // if (buyersInventory.Money == price)
            // {
            //     if (ownersInventory) { ownersInventory.AddMoney(buyersInventory.Money); }
            //     buyersInventory.SubtractMoney(price);

            //     boughtItem = this;
            //     SetOwner(buyer);
            //     return true;
            // }

            // boughtItem = null;
            // return false;
        }

    }
}
