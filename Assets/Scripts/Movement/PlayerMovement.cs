using System.Collections;
using System.Collections.Generic;
using SIM.Control;
using UnityEngine;

namespace SIM.Movement
{
    [RequireComponent(typeof(PlayerControl))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float speed = 2f;

        public Vector2 Forward { get; private set; }

        PlayerControl input;
        Rigidbody2D rb2d;
        Vector2 velocity;

        private void Awake()
        {
            input = GetComponent<PlayerControl>();
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void OnEnable() => input.onPlayerMovement += OnMovementChanged;

        private void OnDisable() => input.onPlayerMovement -= OnMovementChanged;

        private void FixedUpdate()
        {
            rb2d.velocity = velocity;
        }

        private void OnMovementChanged(Vector2 movement)
        {
            Vector2 direction = movement.normalized;
            velocity = direction * speed;

            if (direction != Vector2.zero) Forward = direction;
        }

        private void OnDrawGizmos() {
            const float LENGTH = 3f;
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Forward * LENGTH);
        }
    }
}
