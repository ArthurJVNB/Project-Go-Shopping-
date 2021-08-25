using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SIM.Control;

namespace SIM.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float speed = 2f;
        [SerializeField] Transform directionRepresentation;

        PlayerInput input;
        Rigidbody2D rb2d;
        Vector2 velocity;

        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void OnEnable() => input.onPlayerMovement += OnMovementChanged;

        private void OnDisable() => input.onPlayerMovement -= OnMovementChanged;

        private void FixedUpdate()
        {
            Vector2 finalPosition = (Vector2)transform.position + velocity * Time.fixedDeltaTime;
            rb2d.MovePosition(finalPosition);
        }

        private void OnMovementChanged(Vector2 movement)
        {
            Vector2 direction = movement.normalized;
            velocity = direction * speed;
        }
    }
}
