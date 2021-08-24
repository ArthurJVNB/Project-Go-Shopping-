using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIM.Control;

namespace SIM.Movement
{
    [RequireComponent(typeof(PlayerControl))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float speed = 2f;

        PlayerControl control;
        Rigidbody2D rb2d;
        Vector2 velocity;

        private void Awake()
        {
            control = GetComponent<PlayerControl>();
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            control.onPlayerMovement += OnMovementChanged;
        }

        private void OnDisable()
        {
            control.onPlayerMovement -= OnMovementChanged;
        }

        private void FixedUpdate()
        {
            Vector2 finalPosition = (Vector2)transform.position + velocity * Time.fixedDeltaTime;
            rb2d.MovePosition(finalPosition);
        }

        private void OnMovementChanged(Vector2 movement)
        {
            Vector2 direction = movement.normalized;
            velocity = direction * speed;
            // Vector2 finalPosition = (Vector2)transform.position + velocity;
            // rb2d.MovePosition(finalPosition);
        }
    }
}
