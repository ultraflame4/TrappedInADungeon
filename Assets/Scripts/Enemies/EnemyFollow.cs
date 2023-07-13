using System;
using Entities;
using UnityEngine;

namespace Enemies
{
    public class EnemyFollow : EnemyStateBehaviour
    {
        public EntityBody body;
        public Rigidbody2D rb;
        public bool allowFlight;
        
        [Tooltip("How far will the enemy follow the player before stopping")]
        public float followRange = 5f;
        [Tooltip("How close the enemy needs to be to attack the player")]
        public float attackDist = 0.1f;

        private Transform player;
        private Vector3 directionToPlayer;
        private Vector3 directionToPlayerSnapped;

        private void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
        }
        

        private void FixedUpdate()
        {
            if (!stateActive) return;
            
            // If player is out of range, go back to patrol
            if (Vector3.Distance(transform.position, player.position) > followRange)
            {
                stateManager.TransitionState(EnemyStates.PATROL); 
                return;
            }
            // Direction to player
            directionToPlayer = (player.position - transform.position).normalized;
            // Direction to player but snapped to the x axis. (y=0)
            directionToPlayerSnapped = new Vector3(directionToPlayer.x, 0).normalized;
            
            RotateTowardsPlayer();
            if (CheckPlayerWithinAttackRange())
            {
                rb.velocity = new Vector2(0, allowFlight ? 0 : rb.velocity.y); // Stop immediately when within range
                return;
            }
            
            Vector3 vel = (allowFlight ? directionToPlayer : directionToPlayerSnapped) * body.Speed;
            // Get distance to player and subtract attack distance
            // 0.01f is a small buffer so that the player is actually within the attack range
            float distance = Vector3.Distance(transform.position, player.position) - attackDist + 0.05f;
            // Clamp distance to 0-1. distFactor makes velocity lower the closer the enemy is to the player
            float distFactor = Mathf.Clamp(Mathf.Pow(2 * distance, 2) + 0.2f, 0, 1);
            vel *= distFactor * Time.deltaTime; // Scale velocity by distFactor & deltaTime
            if (!allowFlight) // Preserve y velocity if enemy can't fly
            {
                vel.y = rb.velocity.y;
            }

            rb.velocity = vel;
        }

        public bool CheckPlayerWithinAttackRange()
        {
            if (Vector3.Distance(transform.position, player.position) < attackDist && Vector2.Angle(directionToPlayer, transform.right) < 90)
            {
                stateManager.TransitionState(EnemyStates.ATTACK);
                return true;
            }
            return false;
        }
        private void RotateTowardsPlayer()
        {
            // Rotate towards player
            // snaps direction to x axis
            transform.right = directionToPlayerSnapped;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, attackDist);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, followRange);
        }
        
    }
}