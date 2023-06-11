using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public Rigidbody2D rb;
        public Animator anim;

        private Vector3 direction;
        public float moveSpeed = 10f;
        public float jumpForce = 100f;
        public int jumpTimes = 1;
        private int jumpsLeft = 0;
        private bool toJump;
        private bool isInAir;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        private void Start()
        {
            jumpsLeft = jumpTimes;
        }

        // Update is called once per frame
        void Update()
        {
            direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

            // This line basically checks if it is ok to jump
            // "|| toJump" to prevent Input.GetButtonDown from setting toJump to false
            // We only set toJump to false if we have did the physics in the FixedUpdate
            toJump = (Input.GetButtonDown("Jump") || toJump || Input.GetAxis("Vertical") > 0) && jumpsLeft > 0;

            UpdateAnimationsParameters();
            UpdateSpriteDirection();
        }

        void UpdateAnimationsParameters()
        {
            anim.SetBool(IsWalking, direction.x != 0);
            anim.SetBool(IsJumping, isInAir);
        }

        /**
         * This method flips the sprite if needed base on the current direction.
         */
        void UpdateSpriteDirection()
        {
            Vector3 localScale = transform.localScale;
            if (direction.x > 0)
            {
                if (localScale.x < 0) // Check if the x scale is negative (facing the wrong way)
                {
                    // If yes then flip it
                    // using multiplication because it preserves original scaling
                    localScale.x *= -1;
                }
            }
            else if (direction.x < 0) // Cannot use else here because it then keep flipping to the left
            {
                // The inverse of above
                if (localScale.x > 0)
                {
                    localScale.x *= -1;
                }
            }

            transform.localScale = localScale;
        }

        private void FixedUpdate()
        {
            Vector2 move = rb.velocity;
            move.x = direction.x * moveSpeed * Time.deltaTime;
            if (toJump)
            {
                move.y = jumpForce * Time.deltaTime;
                toJump = false;
                isInAir = true;
                jumpsLeft--;
            }

            rb.velocity = move;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                jumpsLeft = jumpTimes;
                isInAir = false;
            }
        }
    }
}