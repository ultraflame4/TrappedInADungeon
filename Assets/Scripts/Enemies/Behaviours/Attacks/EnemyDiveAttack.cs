using System.Collections;
using Core.Enemies;
using Core.Entities;
using Enemies.Behaviours.Follow;
using PlayerScripts;
using UnityEngine;

namespace Enemies.Behaviours.Attacks
{
    [RequireComponent(typeof(EnemyFollow), typeof(Rigidbody2D), typeof(EntityBody))]
    public class EnemyDiveAttack : EnemyStateBehaviour
    {
        // Component references
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
        
        private Vector3 targetPos; // target position to dive from. Should always be 45 degrees above player
        private Vector2 diveTarget; // The target position to dive to. Aka the player's last calculated position
        private bool isNavigating = false; // whether the enemy is navigating to the target position
        private bool isDiving = false; // whether the enemy is dive attacking the player
        
        private Vector3 playerCheckPos => transform.TransformPoint( playerCheckOffset);
        private Coroutine diveAttackCoroutine;
        private void Start()
        {
            // Get component references (
            follow = GetComponent<EnemyFollow>();
            rb = GetComponent<Rigidbody2D>();
            body = GetComponent<EntityBody>();
        }

        public override void StateEnter()
        {
            
            Vector3 toPlayer = Player.Transform.position - transform.position;
            // Set target position to 45 degrees (in either direction depending on which is closer) above player
            targetPos = Player.Transform.position + new Vector3(toPlayer.x > 0 ? 1 : -1, 1, 0);
            // mark navigating
            isNavigating = true;
        }

        private void FixedUpdate()
        {
            // If the state is not active, do nothing
            if (!stateActive) return;
            
            // If player is out of range, end dive (which also transitions to alert state)
            if (!follow.CheckPlayerWithinAttackRange()) 
            {
                EndDive(); 
                return;
            }
            // If the enemy is navigating, move towards the target position
            if (isNavigating)
            {
                if (MoveToTarget(targetPos))
                {
                    // If the enemy has reached the target position, stop navigating and start the dive attack
                    isNavigating = false;
                    StartDive();
                }
            }
            else if (isDiving)
            {
                // While diving, check if the player has been hit
                CheckPlayerCollided();
                // Move towards the dive target
                if (MoveToTarget(diveTarget))
                {
                    // When diveTarget reached, end dive
                    EndDive();
                }
            }
        }

        private void StartDive()
        {
            // Stop existing dive attack coroutine if it exists
            if (diveAttackCoroutine is not null)
            {
                StopCoroutine(diveAttackCoroutine);
            }
            // Start dive attack coroutine
            diveAttackCoroutine = StartCoroutine(BeginDiveAttack());
        }
        private void EndDive()
        {
            isDiving = false;
            stateManager.TransitionState(EnemyStates.ALERT);
            stateManager.SetAttackAnim(false);
            if (diveAttackCoroutine is not null)
            {
                StopCoroutine(diveAttackCoroutine);
            }
        }
        /// <summary>
        /// Moves towards a target position and stops (and returns true) when it reaches it
        /// </summary>
        bool MoveToTarget(Vector2 targetPosition)
        {
            Vector2 toTarget = (targetPosition - (Vector2)transform.position).normalized;
            // Move towards the target position
            rb.velocity = toTarget * body.Speed * diveSpeedMult * Time.deltaTime;
            // If the enemy is close enough to the target position, stop moving and return true
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f) 
            {
                rb.velocity = Vector2.zero;
                return true;
            }
            return false;
        }
        
        IEnumerator BeginDiveAttack()
        {
            // Wait for diveWaitTime seconds before starting to dive.
            yield return new WaitForSeconds(diveWaitTime);
            // Turn towards player
            follow.RotateTowardsPlayer();
            // Set dive target to player position
            diveTarget = Player.Transform.position;
            // Mark diving
            isDiving = true;
            // Set attack animation
            stateManager.SetAttackAnim(true);
        }

        public override void StateExit()
        {
            // Exit attack animation
            stateManager.SetAttackAnim(false);
        }

        private void CheckPlayerCollided()
        {
            // Overlap circle to check if player is within damage range
            Collider2D playerCollider = Physics2D.OverlapCircle(playerCheckPos, playerCheckRadius, LayerMask.GetMask("Player"));
            if (playerCollider is not null)
            {
                // If player is within damage range, damage the player and end the dive attack
                Player.Body.Damage(body.Attack);
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