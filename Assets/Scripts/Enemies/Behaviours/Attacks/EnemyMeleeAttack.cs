using System;
using Entities;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Melee attack for enemies. The attacks are triggered by animation events. Call MeleeAttack() from the animation event.
    /// </summary>
    [RequireComponent(typeof(EntityBody), typeof(EnemyFollow))]
    public class EnemyMeleeAttack : EnemyStateBehaviour
    {
        private EntityBody entityBody;
        private EnemyFollow follow;
        [Tooltip("Position of the hitbox relative to the enemy")]
        public Vector2 hitboxPosition;
        [Tooltip("Radius of the hitbox")]
        public float hitboxRadius;
        [Tooltip("Damage dealt by the attack")]
        public float baseDamage=10;
        /// <summary>
        /// Position of hitbox in world space
        /// </summary>
        private Vector2 hitboxPos => transform.TransformPoint(hitboxPosition);

        private void Start()
        {
            entityBody = GetComponent<EntityBody>();
            follow = GetComponent<EnemyFollow>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawSphere(hitboxPos, hitboxRadius);
        }

        public override void StateEnter()
        {
            stateManager.SetAttackAnim(true);
        }
        public override void StateExit()
        {
            stateManager.SetAttackAnim(false);
        }

        public void MeleeAttack()
        {
            Collider2D collider = Physics2D.OverlapCircle(hitboxPos,hitboxRadius, LayerMask.GetMask("Player"));
            if (collider is not null)
            {
                collider.GetComponent<EntityBody>().Damage(entityBody.CalculateAttackDamage(baseDamage));
            }
        }

        private void FixedUpdate()
        {
            if (!stateActive) return;
            if (!follow.CheckPlayerWithinAttackRange())
            {
                stateManager.TransitionState(EnemyStates.ALERT);
            }
        }
    }
}