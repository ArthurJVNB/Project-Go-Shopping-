using System;
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

        private void OnEnable() => input.onPlayerPressedInteraction += OnPlayerInteraction;

        private void OnDisable() => input.onPlayerPressedInteraction -= OnPlayerInteraction;

        private void OnPlayerInteraction()
        {
            if (TryToInteract(out IInteractable interactable))
            {
                interactable.Interact(out GameObject interactedGameObject);
            }
        }

        private bool TryToInteract(out IInteractable interactable)
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