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
        public bool CanSale { get { return canSaleNow; } }
        public float Price { get { return GetPrice(); } }
        // public bool IsInGameWorld { get { return isInGameWorld; } set { SetIsInGameWorld(value); } }

        [SerializeField] Sprite worldImage;
        [SerializeField] Sprite inventoryImage;
        [SerializeField] Sprite equippedImage;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Hint hintUI;
        [SerializeField] bool isForSale = true;
        [SerializeField] float price = 100f;
        [SerializeField] State currentState = State.InGameWorld;
        [SerializeField] EquipmentSlot equipmentSlot;
        // [SerializeField] bool isInGameWorld = true;

        const string PLAYER_TAG = "Player";
        bool canSaleNow;
        Trader owner = null;

        private void Awake()
        {
            UpdateHintText();
            UpdateState(currentState);
        }

        #region IInteractable
        public void Interact(GameObject whoInteracts, out GameObject interactedGameObject)
        {
            interactedGameObject = gameObject;

            if (whoInteracts.CompareTag(PLAYER_TAG) && currentState == State.InGameWorld)
            {
                // TryToTrade(whoInteracts.GetComponent<Trader>(), out Item _);
                TryToTrade(whoInteracts.GetComponent<Trader>());
            }
        }

        public void ShowHint()
        {
            if (currentState == State.InGameWorld) hintUI.ShowUI();
        }
        #endregion
        
        #region IEquippable
        public bool Equip(Trader whoIsTryingToEquip, out EquipmentSlot slotToPut)
        {
            bool result = false;
            slotToPut = EquipmentSlot.None;

            if (Owner == whoIsTryingToEquip)
            {
                slotToPut = equipmentSlot;
                UpdateState(State.Equipped);
                result = true;
            }

            return result;
        }
        #endregion
        
        #region ITradable
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

        public bool TryToTrade(Trader buyer)
        {
            bool result = false;

            if (canSaleNow && Owner)
            {
                if (Owner.Trade(this, buyer))
                {
                    UpdateState(State.InInventory);
                    result = true;
                }
            }

            return result;
        }

        // public bool TryToTrade(Trader buyer, out Item boughtItem)
        // {
        //     boughtItem = null;
        //     bool result = false;

        //     if (!isForSale)
        //     {
        //         result = false;
        //         print(name + " is not for sale!");
        //     }
        //     else if (Owner)
        //     {
        //         print(name + " has owner (" + Owner.name + ")");
        //         result = Owner.Trade(this, buyer);
        //         if (result) boughtItem = this;
        //     }
        //     else
        //     {
        //         Inventory buyersInventory = buyer.GetComponent<Inventory>();
        //         result = buyersInventory.SubtractMoney(price);
        //         if (result) boughtItem = this;
        //     }

        //     print("Trade " + (result ? "was successful" : "has failed"));
        //     return result;
        // }
        #endregion

        private void UpdateHintText()
        {
            hintUI.Text = "Buy " + name + " for $" + Price;
        }

        private void UpdateState(State state)
        {
            currentState = state;
            switch (currentState)
            {
                case State.Equipped:
                    SetStateToEquipped();
                    break;
                case State.InInventory:
                    SetStateToInventory();
                    break;
                default:
                    SetStateToWorld();
                    break;
            }
        }

        private void SetStateToEquipped()
        {
            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }

            canSaleNow = false;
            spriteRenderer.sprite = equippedImage;
        }

        private void SetStateToInventory()
        {
            gameObject.SetActive(false);

            canSaleNow = isForSale;
            spriteRenderer.sprite = inventoryImage;
        }

        private void SetStateToWorld()
        {
            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = true;
            }

            canSaleNow = isForSale;
            spriteRenderer.sprite = worldImage;
        }

        // private void SetIsInGameWorld(bool value)
        // {
        //     isInGameWorld = value;
        //     GetComponent<SpriteRenderer>().enabled = value;
        // }
    }
}
