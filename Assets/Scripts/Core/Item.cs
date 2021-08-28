using System;
using System.Collections;
using System.Collections.Generic;
using SIM.UI;
using UnityEngine;

namespace SIM.Core
{
    public class Item : MonoBehaviour, IInteractable, ITradable, IEquippable<Trader>
    {
        [Serializable]
        public enum State
        {
            InGameWorld,
            Equipped,
            InInventory
        }

        public Action<Trader, Trader> onChangedOwner; // old, new

        public Sprite InventoryImage { get { return inventoryImage; } }
        public Trader Owner { get { return GetOwner(); } private set { SetOwner(value); } }
        public bool IsForSale { get { return isForSale; } }
        public float Price { get { return GetPrice(); } }
        // public bool IsInGameWorld { get { return isInGameWorld; } set { SetIsInGameWorld(value); } }

        [SerializeField] Sprite inventoryImage;
        [SerializeField] Sprite inWorldImage;
        [SerializeField] Sprite equippedImage;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Hint hintUI;
        [SerializeField] bool isForSale = true;
        [SerializeField] float price = 100f;
        [SerializeField] State currentState = State.InGameWorld;
        [SerializeField] EquipmentSlot equipmentSlot;
        // [SerializeField] bool isInGameWorld = true;

        const string PLAYER_TAG = "Player";
        Trader owner = null;

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
            }
        }

        public bool Equip(Trader whoIsTryingToEquip, out EquipmentSlot slotToPut)
        {
            slotToPut = EquipmentSlot.None;
            bool result = false;

            if (Owner == whoIsTryingToEquip)
            {
                slotToPut = equipmentSlot;
                UpdateState(State.Equipped);
                result = true;
            }

            return result;
        }

        private void UpdateState(State state)
        {
            currentState = state;
            switch (currentState)
            {
                case State.Equipped:
                    spriteRenderer.sprite = equippedImage;
                    break;
                case State.InInventory:
                    spriteRenderer.sprite = inventoryImage;
                    break;
                default:
                    spriteRenderer.sprite = inWorldImage;
                    break;
            }
        }

        public void ShowHint()
        {
            hintUI.ShowUI();
        }

        public Trader GetOwner()
        {
            return owner;
        }

        public void SetOwner(Trader newOwner)
        {
            Trader oldOwner = owner;
            owner = newOwner;
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
        }

        private void UpdateHintText()
        {
            hintUI.Text = "Buy " + name + " for $" + Price;
        }

        // private void SetIsInGameWorld(bool value)
        // {
        //     isInGameWorld = value;
        //     GetComponent<SpriteRenderer>().enabled = value;
        // }
    }
}
