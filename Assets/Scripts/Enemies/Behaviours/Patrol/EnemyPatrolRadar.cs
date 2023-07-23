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
            if (!stateActive) return;
            Collider2D hit = Physics2D.OverlapCircle(transform.position, radarRange, LayerMask.GetMask("Player"));
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