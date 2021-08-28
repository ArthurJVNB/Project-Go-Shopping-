using System;
using SIM.Control;
using SIM.Core;
using SIM.Movement;
using SIM.UI;
using UnityEngine;

namespace SIM.Character
{
    [RequireComponent(typeof(PlayerActions))]
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(Equipment))]
    [RequireComponent(typeof(Trader))]
    public class PlayerCharacter : MonoBehaviour
    {
        enum State
        {
            Default,
            TalkingToShopkeeper
        }

        [SerializeField] InventoryUI inventoryUI;
        [SerializeField] Hint notEnoughMoneyUI;

        PlayerActions actions;
        Inventory inventory;
        Equipment equipment;
        Trader trader;

        #region Memory
        State currentState = State.Default;
        ShopkeeperControl shopkeeperImTalking = null;
        #endregion

        private void Awake()
        {
            actions = GetComponent<PlayerActions>();
            inventory = GetComponent<Inventory>();
            equipment = GetComponent<Equipment>();
            trader = GetComponent<Trader>();
        }

        private void OnEnable()
        {
            actions.onStartedTalkingToShopkeeper += StartTalkingToShopkeeperState;
            inventory.onInventoryAddedItem += OnInventoryAddedItem;
            inventoryUI.onItemClicked += InteractWithMyItem;
            trader.onNotEnoughMoney += OnNotEnoughMoney;
        }

        private void OnDisable()
        {
            actions.onStartedTalkingToShopkeeper -= StartTalkingToShopkeeperState;
            inventory.onInventoryAddedItem -= OnInventoryAddedItem;
            inventoryUI.onItemClicked -= InteractWithMyItem;
            trader.onNotEnoughMoney -= OnNotEnoughMoney;
        }

        private void OnInventoryAddedItem(Item item)
        {
            item.ChangeStateToInventory();
        }

        private void OnNotEnoughMoney(Item item)
        {
            notEnoughMoneyUI.ShowUI();
        }

        private void InteractWithMyItem(Item item)
        {
            if (currentState == State.Default)
            {
                Equip(item);
                return;
            }

            if (currentState == State.TalkingToShopkeeper)
            {
                TryToSell(item);
            }
        }

        private void Equip(Item item)
        {
            equipment.Equip(item);
        }

        private bool TryToSell(Item myItem)
        {
            bool result = false;

            if (shopkeeperImTalking)
            {
                if (shopkeeperImTalking.TryGetComponent<Trader>(out Trader otherTrader))
                {
                    result = trader.Trade(myItem, otherTrader);
                }
            }

            return result;
        }

        #region State
        private void StartTalkingToShopkeeperState(ShopkeeperControl shopkeeper)
        {
            UpdateState(State.TalkingToShopkeeper);

            shopkeeper.onEndedTalking += StopTalkingToShopkeeperState;
            shopkeeperImTalking = shopkeeper;
        }

        private void StopTalkingToShopkeeperState()
        {
            UpdateState(State.Default);

            shopkeeperImTalking.onEndedTalking -= StopTalkingToShopkeeperState;
            shopkeeperImTalking = null;
        }

        private void UpdateState(State state)
        {
            currentState = state;
        }
    }
    #endregion
}