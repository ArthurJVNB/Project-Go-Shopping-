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
        public enum State
        {
            Default,
            Talking
        }

        public State CurrentState { get { return currentState; } }

        [SerializeField] InventoryUI inventoryUI;
        [SerializeField] RangeDetector rangeToContinueTrade;
        [SerializeField] Hint hintUI;

        const string PLAYER_TAG = "Player";
        Trader trader;
        State currentState = State.Default;
        float distanceFromPlayer;

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
                StopTradingState();
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
                    StartTradingState();
                    return;
                }

                if (currentState == State.Talking)
                {
                    StopTradingState();
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
            if (currentState == State.Default) hintUI.ShowUI();
        }

        private void HideHint()
        {
            hintUI.gameObject.SetActive(false);
        }

        private void StartTradingState()
        {
            HideHint();
            UpdateDistanceFromPlayer();
            inventoryUI.ShowUI();
            currentState = State.Talking;
        }

        private void StopTradingState()
        {
            inventoryUI.HideUI();
            currentState = State.Default;
        }

        private void UpdateDistanceFromPlayer()
        {
            Vector3 playerPosition = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform.position;
            distanceFromPlayer = Vector3.Distance(playerPosition, transform.position);
            print("Distance from player: " + distanceFromPlayer);
        }

        private void SellToPlayer(Item item)
        {
            Trader player = GameObject.FindWithTag("Player").GetComponent<Trader>();
            trader.Trade(item, player);
        }
    }
}