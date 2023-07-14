﻿using System;
using Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class EnemyFollow : EnemyStateBehaviour
    {
        public EntityBody body;
        public Rigidbody2D rb;

        public bool allowFlight;

        [Tooltip("How far will the enemy follow the player before stopping")]
        public float followRange = 5f;

        [FormerlySerializedAs("attackDist")] [Tooltip("Distance to the player before the enemy stops ( and presumably starts attacking )")]
        public float stopDist = 0.1f;

        [Tooltip("Should the enemy be facing the player when attacking?")]
        public bool checkDirection = true;

        private Transform player;
        private Vector3 directionToPlayer;
        private Vector3 directionToPlayerSnapped;

        private void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
        }


        private void FixedUpdate()
        {
            // Direction to player
            directionToPlayer = (player.position - transform.position).normalized;
            // Direction to player but snapped to the x axis. (y=0)
            // Using boolean logic to determine whether to snap to 1 or -1 else when x=0, the Vector normalisation will freak out between 1 and -1
            directionToPlayerSnapped = new Vector3(directionToPlayer.x > 0 ? 1 : -1, 0);

            if (!stateActive) return;
            // If enemy is not grounded and can't fly, skip moving
            if (!stateManager.isGrounded && !allowFlight) return;

            // If player is out of range, go back to patrol
            if (Vector3.Distance(transform.position, player.position) > followRange)
            {
                rb.velocity = Vector2.zero; // Stop moving when player out of range
                stateManager.TransitionState(EnemyStates.PATROL);
                return;
            }


            RotateTowardsPlayer();
            if (CheckPlayerWithinAttackRange())
            {
                rb.velocity = new Vector2(0, allowFlight ? 0 : rb.velocity.y); // Stop immediately when within range
                return;
            }

            Vector3 vel = (allowFlight ? directionToPlayer : directionToPlayerSnapped) * body.Speed;
            // Get distance to player and subtract attack distance
            // 0.01f is a small buffer so that the player is actually within the attack range
            float distance = Vector3.Distance(transform.position, player.position) - stopDist;
            // Clamp distance to 0-1. distFactor makes velocity lower the closer the enemy is to the player
            float distFactor = Mathf.Clamp(Mathf.Pow(2 * distance, 2), 0, 1);
            // Scale velocity by distFactor
            // distance will become negative when player is within stopDist. However because distance gets squared, it will always be positive,
            // and we dont want the enemy to move backwards when the player is within stopDist.
            // Hence we simply do a check to see if distance is negative and make distFactor 0 if it is
            vel *= distance > 0 ? distFactor : 0; 
            
            if (!allowFlight) // Preserve y velocity if enemy can't fly
            {
                vel.y = rb.velocity.y;
            }

            rb.velocity = vel * Time.deltaTime;
        }

        public bool CheckPlayerWithinAttackRange()
        {
            if (Vector3.Distance(transform.position, player.position) <= stopDist)
            {
                // If the enemy should be facing the player when attacking, check if the player is within 45 degrees of the enemy's right
                if (checkDirection && !(Vector2.Angle(directionToPlayer, transform.right) > 45)) return false;
                stateManager.TransitionState(EnemyStates.ATTACK);
                return true;
            }

            // If the enemy can't fly, and  player is within the stop distance, return true but don't transition to attack
            if (!allowFlight && Mathf.Abs(player.position.x - transform.position.x) < stopDist)
            {
                return true;
            }

            return false;
        }

        public void RotateTowardsPlayer()
        {
            // Rotate towards player
            // snaps direction to x axis
            transform.right = directionToPlayerSnapped;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, stopDist);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, followRange);
        }
    }
}