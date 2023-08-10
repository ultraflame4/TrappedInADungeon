using System;
using System.Collections;
using Core.Entities;
using Core.Sound;
using Core.Utils;
using Entities;
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

        [Tooltip("statusEffect applied to entity on hit/explode")]
        public StatusEffect statusEffect;

        [Tooltip("Radius to check for entities to damage on hit/explode")]
        public float damageRadius = 1f;

        [Tooltip("Animator for this projectile")]
        public Animator animator;

        [Tooltip("Sound to play on hit")]
        public SoundEffect hitSound = null;

        private static readonly int AnimIdHit = Animator.StringToHash("hit"); // Animation parameter id

        [Tooltip("Whether this projectile will collide with player (and damage them)")]
        public bool attackPlayer;

        /// <summary>
        /// Maximum amount of time allowed for this projectile before we kill it.
        /// </summary>
        private const float TimeToLiveSecs = 10f;

        private bool isHit;

        private void Start()
        {
            // Start auto destroy after TimeToLiveSecs (TTL) so that it doesnt go on forever
            StartCoroutine(AutoDestroy());
        }

        IEnumerator AutoDestroy()
        {
            // Wait until TimeToLiveSecs (TTL). Then destroy
            yield return new WaitForSeconds(TimeToLiveSecs);
            Destroy(gameObject);
        }

        void FixedUpdate()
        {
            // If projectile has not hit something...
            // Move the projectile in the direction it is facing.
            if (!isHit) transform.position += transform.right * (projectileStats.Speed / 100f) * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // If set to  attack player, and trigger enter was not player, return
            if (attackPlayer != other.CompareTag("Player")) return;
            // Explode animation
            Explode();
            // If projectile hasn't already hit something...
            if (!isHit)
            {
                // Get all colliders in damage radius
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius,
                    LayerMask.GetMask(attackPlayer ? "Player" : "Enemy") // Get all colliders in layer "Player" or "Enemy" depending if we attacking player
                    );
                
                // For each collider, get the entity body and damage it and apply status effect if it exists
                foreach (var collider in colliders)
                {
                    var body = collider.GetComponent<EntityBody>();
                    if (body == null) continue;
                    body.Damage(projectileStats.Attack);
                    if (statusEffect == null) continue;
                    body.AddStatusEffect(statusEffect);
                }
            }
            // Mark projectile as hit
            isHit = true;
        }

        /// <summary>
        /// Plays the explode animation
        /// </summary>
        public void Explode()
        {
            animator.SetTrigger(AnimIdHit);
            hitSound?.PlayAtPoint(transform.position);
        }

        /// <summary>
        /// Destroys this projectile
        /// </summary>
        public void DestroyProjectile()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            // Draw damage radius in editor
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }
    }
}