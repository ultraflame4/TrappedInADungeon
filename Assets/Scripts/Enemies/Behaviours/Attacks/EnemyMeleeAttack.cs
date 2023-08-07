using Core.Enemies;
using Core.Entities;
using Enemies.Behaviours.Follow;
using UnityEngine;

namespace Enemies.Behaviours.Attacks
{
    /// <summary>
    /// Melee attack for enemies. The attacks are triggered by animation events. Call MeleeAttack() from the animation event.
    /// </summary>
    [RequireComponent(typeof(EntityBody), typeof(EnemyFollow))]
    public class EnemyMeleeAttack : EnemyStateBehaviour
    {
        // Component references
        private EntityBody entityBody;
        private EnemyFollow follow;

        [Tooltip("Position of the hitbox relative to the enemy")]
        public Vector2 hitboxPosition;

        [Tooltip("Radius of the hitbox")]
        public float hitboxRadius;

        [Tooltip("Damage dealt by the attack")]
        public float baseDamage = 10;

        /// <summary>
        /// Position of hitbox in world space
        /// </summary>
        private Vector2 hitboxPos => transform.TransformPoint(hitboxPosition);

        private void Start()
        {
            // Get component references
            entityBody = GetComponent<EntityBody>();
            follow = GetComponent<EnemyFollow>();
        }

        private void OnDrawGizmosSelected()
        {
            // Draw hitbox
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawSphere(hitboxPos, hitboxRadius);
        }

        public override void StateEnter()
        {
            // Start attack animation
            stateManager.SetAttackAnim(true);
        }

        public override void StateExit()
        {
            // End attack animation
            stateManager.SetAttackAnim(false);
        }

        public void MeleeAttack()
        {
            // Check if player is within hitbox
            Collider2D collider = Physics2D.OverlapCircle(hitboxPos, hitboxRadius, LayerMask.GetMask("Player"));
            if (collider is not null)
            {
                // If player is within hitbox, deal damage
                collider.GetComponent<EntityBody>().Damage(entityBody.CalculateAttackDamage(baseDamage));
            }
        }

        private void FixedUpdate()
        {
            // if this state not active, return
            if (!stateActive) return;
            // If player is not within attack range, transition to alert state
            if (!follow.CheckPlayerWithinAttackRange()) stateManager.TransitionState(EnemyStates.ALERT);
        }
    }
}