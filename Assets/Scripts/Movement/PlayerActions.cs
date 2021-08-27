using System;
using System.Collections;
using SIM.Control;
using SIM.Core;
using UnityEngine;

namespace SIM.Movement
{
    [RequireComponent(typeof(PlayerControl))]
    [RequireComponent(typeof(PlayerMovement))]

    public class PlayerActions : MonoBehaviour
    {
        enum State
        {
            Default,
            WaitingConfirmation
        }

        [SerializeField] float maxInteractionDistance = 2f;

        PlayerControl input;
        PlayerMovement movement;
        State currentState = State.Default;

        private void Awake()
        {
            input = GetComponent<PlayerControl>();
            movement = GetComponent<PlayerMovement>();
        }

        // private void OnEnable()
        // {
        //     input.onPlayerPressedToInteract += OnPlayerPressedToInteract;
        //     input.onPlayerPressedConfirm += OnPlayerPressedToConfirm;
        // }

        // private void OnDisable()
        // {
        //     input.onPlayerPressedToInteract -= OnPlayerPressedToInteract;
        //     input.onPlayerPressedConfirm -= OnPlayerPressedToConfirm;
        // }

        private void OnEnable() => input.onPlayerPressedToInteract += OnPlayerPressedToInteract;

        private void OnDisable() => input.onPlayerPressedToInteract -= OnPlayerPressedToInteract;

        private void LateUpdate() {
            if (TryGetInteractable(out IInteractable interactable))
            {
                interactable.ShowHint();
            }
        }

        // private void OnPlayerPressedToConfirm()
        // {
        //     if (currentState != State.WaitingConfirmation) return;

        //     if (TryGetInteractable(out IInteractable interactable))
        //     {
        //         interactable.Interact(gameObject, out GameObject interacted);
        //         if (interacted.TryGetComponent<Item>(out Item item))
        //         {
        //             if (item.TryToTrade(this.GetComponent<Trader>(), out Item boughtItem))
        //             {
        //                 print("You bought " + boughtItem.name + "!");
        //             }
        //         }
        //     }
        // }

        private void OnPlayerPressedToInteract()
        {
            // if (currentState != State.Default) return;

            if (TryGetInteractable(out IInteractable interactable))
            {
                interactable.Interact(this.gameObject, out GameObject interactedGameObject);
                // InteractWithWorldItem(interactedGameObject);
            }
        }

        // private void InteractWithWorldItem(GameObject interactedGameObject)
        // {
        //     if (interactedGameObject.TryGetComponent<Item>(out Item item))
        //     {
        //         if (item.IsForSale && item.IsInGameWorld)
        //         {
        //             ShowBuyWindow(item);
        //             StartCoroutine(ContinuesToBeInRange(item));
        //         }
        //     }
        // }

        // private void ShowBuyWindow(Item item)
        // {
        //     print("<WINDOW APPEARS> Do you want to buy " + item.name + " for $" + item.Price + "?");
        // }

        // private IEnumerator ContinuesToBeInRange(Item item)
        // {
        //     yield return null;
        //     currentState = State.WaitingConfirmation;


        //     // input.onPlayerPressedToInteract -= OnPlayerPressedToInteract;
        //     // input.onPlayerPressedConfirm += OnPlayerPressedToConfirm;

        //     while (TryGetInteractable(out IInteractable otherItem))
        //     {
        //         if (otherItem.Equals(item)) yield return null;
        //     }

        //     // input.onPlayerPressedToInteract += OnPlayerPressedToInteract;
        //     // input.onPlayerPressedConfirm -= OnPlayerPressedToConfirm;

        //     currentState = State.Default;
        // }

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