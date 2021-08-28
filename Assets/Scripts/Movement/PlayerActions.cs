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
        public Action<ShopkeeperControl> onStartedTalkingToShopkeeper;

        [SerializeField] float maxInteractionDistance = 2f;

        PlayerControl input;
        PlayerMovement movement;

        private void Awake()
        {
            input = GetComponent<PlayerControl>();
            movement = GetComponent<PlayerMovement>();
        }

        private void OnEnable()
        {
            input.onPlayerPressedToInteract += OnPlayerPressedToInteract;
        }

        private void OnDisable()
        {
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
                        onStartedTalkingToShopkeeper?.Invoke(shopkeeper);
                    }
                }
            }
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
#if UNITY_EDITOR
                    Debug.DrawLine(transform.position, hit.point, Color.blue, .25f);
#endif
                    return true;
                }
            }

#if UNITY_EDITOR
            Debug.DrawRay(transform.position, direction * maxInteractionDistance, Color.yellow);
#endif

            interactable = null;
            return false;
        }
    }
}