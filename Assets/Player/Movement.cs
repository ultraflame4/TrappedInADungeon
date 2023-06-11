using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public Rigidbody2D rb;

        private Vector3 direction;
        public float moveSpeed = 10f;
        public float jumpForce = 100f;
        public int jumpTimes = 1;
        private int jumpsLeft = 0;
        private bool isJumping;
        private void Start()
        {
            jumpsLeft = jumpTimes;
        }

        // Update is called once per frame
        void Update()
        {
            direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            
            // Jumping code
            // or isJumping to prevent Input.GetButtonDown from setting isJumping to false
            // We only set isJumping to false if we have did the physics in the FixedUpdate
            isJumping = (Input.GetButtonDown("Jump") || isJumping) && jumpsLeft > 0;

            
        }
        
        private void FixedUpdate()
        {
            Vector2 move = rb.velocity;
            move.x = direction.x * moveSpeed * Time.deltaTime;
            if (isJumping)
            {
                move.y = jumpForce * Time.deltaTime;
                isJumping = false;
                jumpsLeft--;
            }
            rb.velocity = move;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                jumpsLeft = jumpTimes;
            }
        }
    }
}