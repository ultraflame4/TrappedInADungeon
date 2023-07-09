using Item;
using UnityEngine;

namespace Skills
{
    [RequireComponent(typeof(CircleCollider2D)),RequireComponent(typeof(CircleCollider2D))]
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// The itemInstance that is used to shoot this projectile.
        /// </summary>
        public IItemInstance itemInstance;
        public Vector2 direction;
        
        void FixedUpdate()
        {
            // Move the projectile in the direction it is facing.
            transform.Translate(transform.right * (itemInstance.Speed * Time.deltaTime));
        }
        
    }
}