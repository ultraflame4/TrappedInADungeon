using System;
using Entities;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Melee attack for enemies. The attacks are triggered by animation events. Call MeleeAttack() from the animation event.
    /// </summary>
    public class EnemyMeleeAttack : EnemyStateBehaviour
    {
        public EntityBody entityBody;
        public Vector2 hitboxPosition;
        public float hitboxRadius;
        public float baseDamage=10;
        public EnemyFollow follow;
        private Vector2 hitboxPos => transform.TransformPoint(hitboxPosition);
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