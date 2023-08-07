using Core.Enemies;
using UnityEngine;

namespace Enemies.Behaviours.Patrol
{
    /// <summary>
    /// Similar to EnemyPatrolEye,
    /// Detects player within a certain range. If the player is detected, the enemy will transition to the alert state.
    /// </summary>
    public class EnemyPatrolRadar : EnemyStateBehaviour
    {
        [Tooltip("Range to detect player")]
        public float radarRange = 2f;
        private void FixedUpdate()
        {
            // If the state is not active, do nothing
            if (!stateActive) return;
            // Check if player is within attack range
            Collider2D hit = Physics2D.OverlapCircle(transform.position, radarRange, LayerMask.GetMask("Player"));
            // If hit box overlaps with player, transition to alert state
            if (hit is not null)
            {
                stateManager.TransitionState(EnemyStates.ALERT);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radarRange);
        }
    }
}