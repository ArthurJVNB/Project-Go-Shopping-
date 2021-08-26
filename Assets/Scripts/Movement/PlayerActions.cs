using System;
using System.Collections;
using SIM.Control;
using SIM.Core;
using UnityEngine;

namespace SIM.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerActions : MonoBehaviour
    {
        [SerializeField] float maxInteractionDistance = 2f;

        PlayerInput input;
        PlayerMovement movement;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            movement = GetComponent<PlayerMovement>();
        }

        private void OnEnable() => input.onPlayerPressedToInteract += OnPlayerPressedToInteract;

        private void OnDisable() => input.onPlayerPressedToInteract -= OnPlayerPressedToInteract;

        private void OnPlayerPressedToConfirm()
        {
            if (TryGetInteractable(out IInteractable interactable))
            {
                interactable.Interact(gameObject, out GameObject interacted);
                if (interacted.TryGetComponent<Item>(out Item item))
                {
                    if (item.TryToTrade(this.GetComponent<Trader>(), out Item boughtItem))
                    {
                        print("You bought " + boughtItem.name + "!");
                    }
                }
            }
        }

        private void OnPlayerPressedToInteract()
        {
            if (TryGetInteractable(out IInteractable interactable))
            {
                interactable.Interact(this.gameObject, out GameObject interactedGameObject);
                if (interactedGameObject.TryGetComponent<Item>(out Item item) && item.IsForSale)
                {
                    ShowBuyWindow(item);
                    StartCoroutine(ContinuesToBeInRange(item));
                }
            }
        }

        private void ShowBuyWindow(Item item)
        {
            print("<WINDOW APPEARS> Do you want to buy " + item.name + " for $" + item.Price + "?");
        }

        private IEnumerator ContinuesToBeInRange(Item item)
        {
            yield return null;
            input.onPlayerPressedToInteract -= OnPlayerPressedToInteract;
            input.onPlayerPressedConfirm += OnPlayerPressedToConfirm;
            while (TryGetInteractable(out IInteractable otherItem))
            {
                if (otherItem.Equals(item)) yield return null;
            }
            input.onPlayerPressedToInteract += OnPlayerPressedToInteract;
            input.onPlayerPressedConfirm -= OnPlayerPressedToConfirm;
        }

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
                    Debug.DrawLine(transform.position, hit.transform.position, Color.blue, .25f);
                    return true;
                }
            }

            Debug.DrawRay(transform.position, direction * maxInteractionDistance, Color.yellow, .25f);
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