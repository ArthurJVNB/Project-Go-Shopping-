using System;
using SIM.Control;
using SIM.Core;
using UnityEngine;

namespace SIM.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerActions : MonoBehaviour
    {
        [SerializeField] float maxInteractionDistance = 2f;

        PlayerInput input;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();

        }

        private void OnEnable() => input.onPlayerPressedInteraction += OnPlayerInteraction;

        private void OnDisable() => input.onPlayerPressedInteraction -= OnPlayerInteraction;

        private void OnPlayerInteraction()
        {
            if (TryToInteract(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }

        private bool TryToInteract(out IInteractable interactable)
        {
            Ray ray = new Ray(transform.position, Vector3.right);

            Debug.DrawRay(transform.position, Vector3.right * maxInteractionDistance, Color.green, .5f);

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, maxInteractionDistance);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject.Equals(this.gameObject)) continue; // Typed "this.gameObject" to be more explicit for the reader.

                if (hit.transform.TryGetComponent<IInteractable>(out interactable))
                {
                    return true;
                }
            }

            interactable = null;
            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, maxInteractionDistance);
        }
    }
}