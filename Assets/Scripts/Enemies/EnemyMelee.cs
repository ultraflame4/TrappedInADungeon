using Entities;
using UnityEngine;

namespace Enemies
{
    public class EnemyMelee : MonoBehaviour
    {
        public EntityBody entityBody;
        public Vector2 hitboxPosition;
        public float hitboxRadius;
        public float baseDamage=10;
        
        private Vector2 hitboxPos => transform.TransformPoint(hitboxPosition);
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawSphere(hitboxPos, hitboxRadius);
        }

        public void MeleeAttack()
        {
            Collider2D collider = Physics2D.OverlapCircle(hitboxPos,hitboxRadius, LayerMask.GetMask("Player"));
            if (collider is not null)
            {
                collider.GetComponent<EntityBody>().Damage(entityBody.CalculateAttackDamage(baseDamage));
            }
            
        }
    }
}