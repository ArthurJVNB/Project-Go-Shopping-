using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SIM.Control
{
    public class PlayerControl : MonoBehaviour
    {
        public Action<Vector2> onPlayerMovement;
        public Action onPlayerPressedConfirm;
        public Action onPlayerPressedCancel;
        public Action onPlayerPressedToInteract;

        private void Update()
        {
            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            onPlayerMovement?.Invoke(direction);

            if (Input.GetButtonDown("Interact"))
            {
                onPlayerPressedToInteract?.Invoke();
            }

            if (Input.GetButtonDown("Submit"))
            {
                onPlayerPressedConfirm?.Invoke();
            }

            if (Input.GetButtonDown("Cancel"))
            {
                onPlayerPressedCancel?.Invoke();
            }
        }
    }
}
