﻿using System;
using System.Collections;
using Entities;
using Player;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyFollow), typeof(Rigidbody2D), typeof(EntityBody))]
    public class EnemyDiveAttack : EnemyStateBehaviour
    {
        private EnemyFollow follow;
        private Rigidbody2D rb;
        private EntityBody body;
        
        [Tooltip("Time to wait in seconds before the enemy dive attacks the player")]
        public float diveWaitTime = 0.5f;
        [Tooltip("Multiplier for the speed of the enemy when diving")]
        public float diveSpeedMult = 2f;
        [Header("Player Check")]
        [Tooltip("Radius of the circle used to check if the player is within damage range")]
        public float playerCheckRadius = 0.1f;
        [Tooltip("Offset of the circle used to check if the player is within damage range")]
        public Vector2 playerCheckOffset;
        
        private Transform player;
        private Vector3 targetPos; // target position to dive from. Should always be 45 degrees above player
        private Vector2 diveTarget; // The target position to dive to. Aka the player's last calculated position
        private bool isNavigating = false; // whether the enemy is navigating to the target position
        private bool isDiving = false; // whether the enemy is dive attacking the player
        
        private Vector3 playerCheckPos => transform.TransformPoint( playerCheckOffset);
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            follow = GetComponent<EnemyFollow>();
            rb = GetComponent<Rigidbody2D>();
            body = GetComponent<EntityBody>();
        }

        public override void StateEnter()
        {
            Vector3 toPlayer = player.position - transform.position;
            targetPos = player.transform.position + new Vector3(toPlayer.x > 0 ? 1 : -1, 1, 0);
            isNavigating = true;
        }

        private void FixedUpdate()
        {
            if (!stateActive) return;
            if (isNavigating)
            {
                if (MoveToTarget(targetPos))
                {
                    isNavigating = false;
                    StartCoroutine(BeginDiveAttack());
                }
            }
            else if (isDiving)
            {
                CheckPlayerCollided();
                if (MoveToTarget(diveTarget))
                {
                    EndDive();
                }
            }
        }

        private void EndDive()
        {
            isDiving = false;
            stateManager.TransitionState(EnemyStates.ALERT);
            stateManager.SetAttackAnim(false);
        }
        /// <summary>
        /// Moves towards a target position and stops (and returns true) when it reaches it
        /// </summary>
        bool MoveToTarget(Vector2 targetPosition)
        {
            Vector2 toTarget = (targetPosition - (Vector2)transform.position).normalized;
            rb.velocity = toTarget * body.Speed * diveSpeedMult * Time.deltaTime;
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                rb.velocity = Vector2.zero;
                return true;
            }
            return false;
        }
        
        IEnumerator BeginDiveAttack()
        {
            yield return new WaitForSeconds(diveWaitTime);
            follow.RotateTowardsPlayer();
            diveTarget = player.position;
            isDiving = true;
            stateManager.SetAttackAnim(true);
        }

        public override void StateExit()
        {
            stateManager.SetAttackAnim(false);
        }

        private void CheckPlayerCollided()
        {
            Collider2D playerCollider = Physics2D.OverlapCircle(playerCheckPos, playerCheckRadius, LayerMask.GetMask("Player"));
            if (playerCollider is not null)
            {
                player.GetComponent<EntityBody>().Damage(body.Attack);
                EndDive(); // End the dive attack if the player is hit
            }
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawSphere(playerCheckPos, playerCheckRadius);
        }
    }
}