using System;
using System.Collections;
using Entities;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public Rigidbody2D rb;
        public Animator anim;
        public PlayerBody body;
        [Header("Movement")]
        [ReadOnly] public Vector3 currentDirection = Vector3.right;
        public float moveSpeed = 10f;
        [Header("Dash")]
        public float dashSpeed = 100f;
        [Tooltip("The period of time the player will dash for in ms")]
        public float dashPeriod = 1f;
        [Tooltip("% of total mana consumed by dash")]
        public float dashManaCostPercent = 0.2f;

        public float jumpForce = 100f;
        public int jumpTimes = 1;
        private int jumpsLeft = 0;

        private float inputFactor;
        private bool toJump;
        private bool toDash;
        private bool isDashing;
        private bool isInAir;
        private Coroutine dashCoroutine;

        private bool alreadyJumping = false;

        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        private float dashCost => body.Mana * dashManaCostPercent;
        private void Start()
        {
            jumpsLeft = jumpTimes;
            currentDirection=Vector3.right;
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePlayerDirection();

            // This line basically checks if it is ok to jump
            // include toJump in its own condition to prevent Input.GetButtonDown from setting toJump to false
            // We only set toJump to false if we have did the physics in the FixedUpdate
            toJump = GameManager.Controls.Player.Jump.triggered || toJump;
            if (GameManager.Controls.Player.Dash.triggered && body.CurrentMana.value > dashCost)
            {
                toDash = true;
                // Dash consumes 5% of mana or 10 whichever is greater
                body.CurrentMana.value -= dashCost;
            }

            if (GameManager.Controls.Player.Dash.WasReleasedThisFrame()) // If dash button is released, stop dashing
            {
                toDash = false; // Reset dash variables
                isDashing = false;
                if (dashCoroutine != null)
                {
                    StopCoroutine(dashCoroutine); // Stop end dash coroutine
                }
            }
            UpdateAnimationsParameters();
            UpdateSpriteDirection();
        }

        void UpdatePlayerDirection()
        {
            float horizontal = GameManager.Controls.Player.Movement.ReadValue<float>();
            inputFactor = 0f;
            // Only change current direction when there is input
            if (horizontal == 0) return;
            if (isDashing) return;
            currentDirection = new Vector3(horizontal, 0, 0);
            inputFactor = 1;
        }

        void UpdateAnimationsParameters()
        {
            anim.SetBool(IsWalking, inputFactor != 0);
            anim.SetBool(IsJumping, isInAir || toDash);
        }

        /**
         * This method rotates the sprite if needed base on the current direction.
         */
        void UpdateSpriteDirection()
        {
            Quaternion localRotation = transform.localRotation;
            if (currentDirection.x > 0)
            {
                localRotation.eulerAngles = Vector3.up * 0;
            }
            else if (currentDirection.x < 0) // Cannot use else here because it then keep flipping to the left
            {
                localRotation.eulerAngles = Vector3.up * 180;
            }

            transform.localRotation = localRotation;
        }

        private void FixedUpdate()
        {
            Vector2 move = rb.velocity;
            move.x = currentDirection.x * inputFactor * moveSpeed * Time.deltaTime;

            if (toDash)
            {
                // Use normalised direction here because GetAxis does not return discrete values even for keys
                // This results in the movement speed slowly increasing which is nice.
                // But for dashing, we are using currentDirection as well the direction. Not really caring about the smoothing
                // Normalising it will change the direction to length one, aka it will consistent and not be literally 0.00...001
                // Hence making this dashing thing work
                move.x = currentDirection.normalized.x * dashSpeed * Time.deltaTime;
                if (!isDashing) // If just started dashing, start the end dash coroutine
                {
                    if (dashCoroutine is not null) // If there is a dash coroutine running, stop it
                    {
                        isDashing = false;
                        StopCoroutine(dashCoroutine); // Stop end dash coroutine
                    }
                    dashCoroutine=StartCoroutine(EndDash()); // Start end dash coroutine
                }

                isDashing = true;
            }

            // !alreadyJumping -> Dont jump again if alreadyJumping ( when the jump key is pressed but not released yet)
            if (toJump && jumpsLeft > 0 && !alreadyJumping)
            {
                move.y = jumpForce * Time.deltaTime;
                isInAir = true;
                jumpsLeft--;
                alreadyJumping = true;
            }

            // On jump button release, toJump will be false. Reset alreadyJumping
            if (!toJump)
            {
                alreadyJumping = false;
            }

            // Always set toJump to false at the end, regardless whether the jump happened.
            // This fixes a bug where if the player tries the jump again when jumpsLeft = 0, the moment the player touches the ground they jump again.
            toJump = false;
            rb.velocity = move;
        }

        IEnumerator EndDash()
        {
            yield return new WaitForSeconds(dashPeriod / 1000);
            toDash = false;
            isDashing = false;
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