using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SIM.Control
{
    public class PlayerControl : MonoBehaviour
    {
        public Action<Vector2> onPlayerMovement;

        private void Update()
        {
            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            onPlayerMovement?.Invoke(direction);
        }
    }
}
