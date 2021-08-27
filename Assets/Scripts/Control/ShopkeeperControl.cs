using UnityEngine;
using SIM.Core;
using SIM.UI;
using UnityEngine.UI;
using System;

namespace SIM.Control
{
    [RequireComponent(typeof(Trader))]
    public class ShopkeeperControl : MonoBehaviour, IInteractable
    {
        enum State
        {
            Default,
            Talking
        }

        [SerializeField] InventoryUI inventoryUI;
        [SerializeField] RangeDetector rangeToContinueTrade;

        const string PLAYER_TAG = "Player";
        Trader trader;
        State currentState = State.Default;

        private void Awake()
        {
            trader = GetComponent<Trader>();
        }

        private void OnEnable()
        {
            inventoryUI.onItemClicked += SellToPlayer;
            rangeToContinueTrade.onTriggerExit2D += HideInventoryIfPlayerLeft;
        }

        private void OnDisable()
        {
            inventoryUI.onItemClicked -= SellToPlayer;
            rangeToContinueTrade.onTriggerExit2D -= HideInventoryIfPlayerLeft;
        }

        private void HideInventoryIfPlayerLeft(Collider2D whoTriggered, Collider2D other)
        {
            if (other.CompareTag(PLAYER_TAG))
            {
                currentState = State.Default;
                inventoryUI.HideUI();
            }
        }

        public void Interact(GameObject whoInteracts, out GameObject interacted)
        {
            interacted = gameObject;
            print("<INVENTORY APPEARS> I supply only the finest goods");

            if (whoInteracts.CompareTag(PLAYER_TAG))
            {
                if (currentState == State.Default)
                {
                    inventoryUI.ShowUI();
                    currentState = State.Talking;
                    return;
                }

                if (currentState == State.Talking)
                {
                    inventoryUI.HideUI();
                    currentState = State.Default;
                    return;
                }

                // inventoryUI.SwitchUI();

                // Trader player = whoInteracts.GetComponent<Trader>();
                // Item itemFromPlayer = player.Inventory.GetItems()[0];
                // if (itemFromPlayer)
                // {
                //     player.Trade(itemFromPlayer, this.trader);
                // }
            }
        }

        public void ShowHint()
        {
            if (currentState == State.Default) print("<HINT> Talk");
        }

        private void SellToPlayer(Item item)
        {
            Trader player = GameObject.FindWithTag("Player").GetComponent<Trader>();
            trader.Trade(item, player);
        }
    }
}