using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public Rigidbody2D rb;
        public Animator anim;

        private Vector3 currentDirection;
        public float moveSpeed = 10f;
        public float dashSpeed = 100f;

        public float jumpForce = 100f;
        public int jumpTimes = 1;
        private int jumpsLeft = 0;

        private float toMove; // is float so i can use in multiplication
        private bool toJump;
        private bool toDash;
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
            float vertical = Input.GetAxis("Horizontal");
            toMove = 0f;
            if (vertical != 0) // Only change current direction when there is input
            {
                currentDirection = new Vector3(vertical, 0, 0);
                toMove = 1;
            }

            // This line basically checks if it is ok to jump
            // include toJump in its own condition to prevent Input.GetButtonDown from setting toJump to false
            // We only set toJump to false if we have did the physics in the FixedUpdate
            toJump = Input.GetButtonDown("Jump") || toJump || Input.GetAxis("Vertical") > 0;
            toDash = Input.GetButtonDown("Dash") || toDash;
            UpdateAnimationsParameters();
            UpdateSpriteDirection();
        }

        void UpdateAnimationsParameters()
        {
            anim.SetBool(IsWalking, toMove != 0);
            anim.SetBool(IsJumping, isInAir || toDash);
        }

        /**
         * This method flips the sprite if needed base on the current direction.
         */
        void UpdateSpriteDirection()
        {
            Vector3 localScale = transform.localScale;
            if (currentDirection.x > 0)
            {
                if (localScale.x < 0) // Check if the x scale is negative (facing the wrong way)
                {
                    // If yes then flip it
                    // using multiplication because it preserves original scaling
                    localScale.x *= -1;
                }
            }
            else if (currentDirection.x < 0) // Cannot use else here because it then keep flipping to the left
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
            move.x = currentDirection.x * toMove * moveSpeed * Time.deltaTime;

            if (toDash)
            {
                // Use normalised direction here because GetAxis does not return discrete values even for keys
                // This results in the movement speed slowly increasing which is nice.
                // But for dashing, we are using currentDirection as well the direction. Not really caring about the smoothing
                // Normalising it will change the direction to length one, aka it will consistent and not be literally 0.00...001
                // Hence making this dashing thing work
                move.x = currentDirection.normalized.x * dashSpeed * Time.deltaTime;
                toDash = false;
            }

            if (toJump && jumpsLeft > 0)
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