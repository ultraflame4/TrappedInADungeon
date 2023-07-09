using System;
using Entities;
using Item;
using UnityEngine;

namespace Projectile
{
    [RequireComponent(typeof(CircleCollider2D)), RequireComponent(typeof(Animator))]
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Stats that affect this projectile
        /// </summary>
        public IEntityStats projectileStats;

        public Animator animator;
        private static readonly int Explode = Animator.StringToHash("hit");
        [Tooltip("Whether this projectile will collide with player (and damage them)")]
        public bool attackPlayer;

        void FixedUpdate()
        {
            // Move the projectile in the direction it is facing.
            transform.Translate(transform.right * (projectileStats.Speed * Time.deltaTime));
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!attackPlayer && other.CompareTag("Player")) return;
            animator.SetTrigger(Explode);
        }
        
        public void DestroyProjectile()
        {
            Destroy(gameObject);
        }
    }
}