using System;
using System.Collections;
using System.Collections.Generic;
using SIM.UI;
using UnityEngine;

namespace SIM.Core
{
    public class Item : MonoBehaviour, IInteractable, ITradable
    {
        public Action<Trader, Trader> onChangedOwner; // old, new

        public bool IsStackable { get { return isStackable; } }
        public Sprite UIImage { get { return uiImage; } }
        public Trader Owner { get; private set; }
        public bool IsForSale { get { return isForSale; } }
        public float Price { get { return GetPrice(); } }
        public bool IsInGameWorld { get { return isInGameWorld; } set { SetIsInGameWorld(value); } }

        [SerializeField] Sprite uiImage;
        [SerializeField] Sprite worldImage;
        [SerializeField] Sprite equippedImage;
        [SerializeField] Hint hintUI;
        [SerializeField] bool isStackable;
        [SerializeField] bool isForSale = true;
        [SerializeField] float price = 100f;
        [SerializeField] bool isInGameWorld = true;

        const string PLAYER_TAG = "Player";

        private void Awake()
        {
            UpdateHintText();
        }

        public void Interact(GameObject whoInteracts, out GameObject interactedGameObject)
        {
            interactedGameObject = gameObject;

            if (whoInteracts.CompareTag(PLAYER_TAG) && isForSale)
            {
                TryToTrade(whoInteracts.GetComponent<Trader>(), out Item _);

                // timesPlayerInteractedWithMe++;

                // if (timesPlayerInteractedWithMe == 1)
                // {
                //     ShowBuyWindow();
                // }
                // else if (timesPlayerInteractedWithMe > 1)
                // {
                //     ResetTimesPlayerInteractedWithMe();
                //     TryToTrade(whoInteracts.GetComponent<Trader>(), out Item _);
                // }
            }

            // interactedGameObject = null;

            // if (isForSale && whoInteracts.CompareTag("Player"))
            // {
            //     interactedGameObject = gameObject;
            // }
        }

        public void ShowHint()
        {
            // print("<WINDOW APPEARS> Do you want to buy " + name + " for $" + Price + "?");
            // Buy T-shirt for $1000
            hintUI.ShowUI();
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
                print(name + " is not for sale!");
            }
            else if (Owner)
            {
                print(name + " has owner (" + Owner.name + ")");
                result = Owner.Trade(this, buyer);
                if (result) boughtItem = this;
            }
            else
            {
                Inventory buyersInventory = buyer.GetComponent<Inventory>();
                result = buyersInventory.SubtractMoney(price);
                if (result) boughtItem = this;
            }

            print("Trade " + (result ? "was successful" : "has failed"));
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

        private void UpdateHintText()
        {
            hintUI.Text = "Buy " + name + " for $" + Price;
        }

        private void SetIsInGameWorld(bool value)
        {
            isInGameWorld = value;
            GetComponent<SpriteRenderer>().enabled = value;
        }
    }
}
