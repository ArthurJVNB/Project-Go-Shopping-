using System;
using System.Collections;
using SIM.Control;
using SIM.Core;
using SIM.UI;
using UnityEngine;

namespace SIM.Movement
{
    [RequireComponent(typeof(PlayerControl))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(Equipment))]
    [RequireComponent(typeof(Trader))]

    public class PlayerActions : MonoBehaviour
    {
        // enum State
        // {
        //     Default,
        //     TalkingToShopkeeper
        // }

        public Action<ShopkeeperControl> onStartedTalkingToShopkeeper;

        [SerializeField] float maxInteractionDistance = 2f;
        // [SerializeField] InventoryUI inventoryUI;

        PlayerControl input;
        PlayerMovement movement;
        // Equipment equipment;
        // Trader trader;
        // State currentState = State.Default;
        // ShopkeeperControl shopkeeperImTalking = null;

        private void Awake()
        {
            input = GetComponent<PlayerControl>();
            movement = GetComponent<PlayerMovement>();
            // equipment = GetComponent<Equipment>();
            // trader = GetComponent<Trader>();
        }

        private void OnEnable()
        {
            // inventoryUI.onItemClicked += InteractWithMyItem;
            input.onPlayerPressedToInteract += OnPlayerPressedToInteract;
        }

        private void OnDisable()
        {
            // inventoryUI.onItemClicked -= InteractWithMyItem;
            input.onPlayerPressedToInteract -= OnPlayerPressedToInteract;
        }

        private void LateUpdate()
        {
            if (TryGetInteractable(out IInteractable interactable))
            {
                interactable.ShowHint();
            }
        }

        private void OnPlayerPressedToInteract()
        {
            if (TryGetInteractable(out IInteractable interactable))
            {
                interactable.Interact(this.gameObject, out GameObject interactedGameObject);
                if (interactedGameObject.TryGetComponent<ShopkeeperControl>(out ShopkeeperControl shopkeeper))
                {
                    if (shopkeeper.CurrentState == ShopkeeperControl.State.Talking)
                    {
                        // StartTalkingToShopkeeperState(shopkeeper);
                        onStartedTalkingToShopkeeper?.Invoke(shopkeeper);
                    }
                }
            }
        }
/*
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
*/
/*
        private void Equip(Item item)
        {
            if (item.Equip(this.trader, out EquipmentSlot slotToPut))
            {
                // print("I'm equipping " + item.name + " on slot " + slotToPut);
                equipment.Equip(item);
            }
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
*/
/*
        private void StartTalkingToShopkeeperState(ShopkeeperControl shopkeeper)
        {
            currentState = State.TalkingToShopkeeper;

            shopkeeper.onEndedTalking += StopTalkingToShopkeeperState;
            shopkeeperImTalking = shopkeeper;
        }

        private void StopTalkingToShopkeeperState()
        {
            currentState = State.Default;

            shopkeeperImTalking.onEndedTalking -= StopTalkingToShopkeeperState;
            shopkeeperImTalking = null;
        }
*/
        private bool TryGetInteractable(out IInteractable interactable)
        {
            Vector2 direction = movement.Forward;
            Ray ray = new Ray(transform.position, direction);

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, maxInteractionDistance);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject.Equals(this.gameObject)) continue; // Typed "this.gameObject" to be more explicit for the reader.

                if (hit.transform.TryGetComponent<IInteractable>(out interactable))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.blue, .25f);
                    return true;
                }
            }

            Debug.DrawRay(transform.position, direction * maxInteractionDistance, Color.yellow);
            interactable = null;
            return false;
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.blue;
        //     Gizmos.DrawWireSphere(transform.position, maxInteractionDistance);
        // }
    }
}