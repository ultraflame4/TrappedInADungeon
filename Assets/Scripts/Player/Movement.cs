using System;
using System.Collections;
using Core.UI;
using Core.Utils;
using Entities;
using UI;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScripts
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator)), RequireComponent(typeof(PlayerBody))]
    public class Movement : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Animator anim;
        private PlayerBody body;

        [Header("Movement")]
        [ReadOnly]
        public Vector3 currentDirection = Vector3.right;

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

        private float inputFactor; // 0 if no input, 1 if input
        private bool toJump; // Whether the character should be jumping
        private bool toDash; // Whether the character should be dashing
        private bool isDashing; // Whether the character is currently dashing
        private bool isInAir; // Whether the character is in the air
        private Coroutine dashCoroutine; // Coroutine for ending the dash

        private bool alreadyJumping = false; // Whether the player is already jumping. Used to prevent jumping when no jumps left.

        private static readonly int AnimIdWalking = Animator.StringToHash("IsWalking"); // Apparently this is better then just using the raw string value
        private static readonly int AnimIdJumping = Animator.StringToHash("IsJumping");

        public event Action DashEvent;
        public event Action JumpEvent;
        public event Action LandEvent;
        public bool IsWalking => inputFactor != 0 && !isInAir;
        private float dashCost => body.Mana * dashManaCostPercent;

        private void Awake()
        {
            body = GetComponent<PlayerBody>();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            jumpsLeft = jumpTimes;
            currentDirection = Vector3.right;
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePlayerDirection();

            // This line basically checks if it is ok to jump
            // include toJump in its own condition to prevent Input.GetButtonDown from setting toJump to false
            // We only set toJump to false if we have did the physics in the FixedUpdate
            toJump = GameManager.Controls.Player.Jump.triggered || toJump;
            if (GameManager.Controls.Player.Dash.triggered)
            {
                if (body.CurrentMana.value > dashCost)
                {
                    body.invulnerable = true;
                    toDash = true;
                    // Dash consumes 5% of mana or 10 whichever is greater
                    body.CurrentMana.value -= dashCost;
                }
                else
                {
                    NotificationManager.Instance.PushNotification("Not enough mana to dash!", addData: $"<color=\"yellow\">({body.CurrentMana.value.ToPrecision(2)}/{dashCost})</color>");
                }
            }

            if (GameManager.Controls.Player.Dash.WasReleasedThisFrame()) // If dash button is released, stop dashing
            {
                body.invulnerable = false;
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
            anim.SetBool(AnimIdWalking, IsWalking);
            anim.SetBool(AnimIdJumping, isInAir || toDash);
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
                    DashEvent?.Invoke();
                    if (dashCoroutine is not null) // If there is a dash coroutine running, stop it
                    {
                        isDashing = false;
                        StopCoroutine(dashCoroutine); // Stop end dash coroutine
                    }

                    dashCoroutine = StartCoroutine(EndDash()); // Start end dash coroutine
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
                JumpEvent?.Invoke();
            }

            // On jump button release, toJump will be false. Reset alreadyJumping
            if (!toJump)
            {
                alreadyJumping = false;
            }

            // Always set toJump to false at the end, regardless whether the jump happened.
            // This fixes a bug_ where if the player tries the jump again when jumpsLeft = 0, the moment the player touches the ground they jump again.
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
                LandEvent?.Invoke();
            }
        }
    }
}