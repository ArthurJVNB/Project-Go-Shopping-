using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SIM.Control
{
    public class PlayerInput : MonoBehaviour
    {
        public Action<Vector2> onPlayerMovement;
        public Action onPlayerPressedInteraction;

        private void Update()
        {
            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            onPlayerMovement?.Invoke(direction);

            if (Input.GetButtonDown("Fire1"))
            {
                onPlayerPressedInteraction?.Invoke();
            }
        }
    }
}
