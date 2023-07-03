﻿using System;
using System.Collections;
using Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class EnemyController : MonoBehaviour
    {
        public Rigidbody2D rb;
        public EntityBody body;
        public Transform player;
        public Animator animator;
        public SpriteRenderer spriteRenderer;

        /// <summary>
        /// Moves directly towards player. Set this to true if enemy can fly.
        /// </summary>
        public bool allowFlight = false;

        public float knockbackForce = 100f;
        public EnemyState state;
        
        public float followRange = 5f;
        public float eyeSightRange = 2f;
        public float eyeSightOffset = 0.5f;

        /// <summary>
        /// How close the enemy needs to be to attack the player
        /// </summary>
        public float attackDist = 0.1f;

        private Vector3 directionToPlayer;
        /// <summary>
        /// Direction to the player but snapped to the x axis. (y=0)
        /// </summary>
        private Vector3 directionToPlayerSnapped;

        // todo implement state machine ai stuff
        Vector3 raycastOrigin => transform.position + Vector3.up * eyeSightOffset;
        /// <summary>
        /// True if attack animation is playing
        /// </summary>
        bool isAttacking => animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        private void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
            body.OnDamagedEvent += OnAttacked;
        }

        public void OnAttacked()
        {
            rb.velocity = (Vector3.up - directionToPlayer / 4).normalized * knockbackForce;
            Stun(500);
        }
        
        /// <summary>
        /// Stuns this enemy for a duration in milliseconds
        /// </summary>
        /// <param name="durationMS">Stun duration in milliseconds</param>
        public void Stun(float durationMS)
        {
            StartCoroutine(StunCoroutine(durationMS));
        }
        private IEnumerator StunCoroutine(float durationMS)
        {
            spriteRenderer.color = Color.grey;
            animator.SetTrigger("Stun");
            state = EnemyState.STUNNED;
            yield return new WaitForSeconds(durationMS / 1000);
            state = EnemyState.ALERT;
            spriteRenderer.color = Color.white;
        }
        private void Update()
        {
            animator.SetBool("isWalking", rb.velocity.magnitude > 0);
            animator.SetBool("Attack", state == EnemyState.ATTACK);
        }

        private void FixedUpdate()
        {
            directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayerSnapped = new Vector3(directionToPlayer.x, 0).normalized;
            switch (state)
            {
                case EnemyState.STUNNED:
                    break;
                case EnemyState.PATROL:
                    CheckPlayerVisible();
                    break;
                case EnemyState.ALERT:
                    if (isAttacking) break;
                    if (Vector3.Distance(transform.position,player.position) > followRange)
                    {
                        state = EnemyState.PATROL;
                        break;
                    }
                    MoveTowardsPlayer();
                    break;

                case EnemyState.ATTACK:
                    if (!CheckPlayerWithinAttackRange())
                    {
                        state = EnemyState.ALERT;
                    }
                    else
                    {
                        RotateTowardsPlayer();
                        rb.velocity = Vector2.zero; // Stop immediately when withing range
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
                }
            }

        public void CheckPlayerVisible()
        {
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, transform.right, eyeSightRange, LayerMask.GetMask("Player"));
            if (!hit.transform) return;
            if (hit.transform.CompareTag("Player"))
            {
                state = EnemyState.ALERT;
            }
        }

        public void RotateTowardsPlayer()
        {
            // Rotate towards player
            // snaps direction to x axis
            transform.right = directionToPlayerSnapped;
        }
        private void MoveTowardsPlayer()
        {
            RotateTowardsPlayer();
            if (CheckPlayerWithinAttackRange())
            {
                // rb.velocity = Vector2.zero;
                return;
            }

            Vector3 vel = (allowFlight ? directionToPlayer : directionToPlayerSnapped) * body.Speed;
            // Get distance to player and subtract attack distance
            // 0.01f is a small buffer so that the player is actually within the attack range
            float distance = Vector3.Distance(transform.position, player.position) - attackDist + 0.05f;
            // Clamp distance to 0-1. distFactor makes velocity lower the closer the enemy is to the player
            float distFactor = Mathf.Clamp(Mathf.Pow(2*distance,2)+0.2f, 0, 1);
            vel *= distFactor * Time.deltaTime; // Scale velocity by distFactor & deltaTime
            if (!allowFlight) // Preserve y velocity if enemy can't fly
            {
                vel.y = rb.velocity.y;
            }
            rb.velocity = vel;
        }
        private bool CheckPlayerWithinAttackRange()
        {
            Vector3 playerPos = player.position;
            if (!allowFlight)
            {
                playerPos.y = transform.position.y;
            }
            if (Vector3.Distance(transform.position,playerPos) < attackDist)
            {
                state = EnemyState.ATTACK;
                return true;
            }

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(raycastOrigin, transform.right * eyeSightRange);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, attackDist);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, followRange);
        }
    }
}