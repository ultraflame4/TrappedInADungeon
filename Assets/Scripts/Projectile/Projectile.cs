using System;
using System.Collections;
using Core.Entities;
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
        public float damageRadius = 1f;
        public Animator animator;
        private static readonly int Explode = Animator.StringToHash("hit");

        [Tooltip("Whether this projectile will collide with player (and damage them)")]
        public bool attackPlayer;
        /// <summary>
        /// Maximum amount of time allowed for this projectile before we kill it.
        /// </summary>
        private const float TimeToLiveSecs = 10f;
        private bool isHit;

        private void Start()
        {
            StartCoroutine(AutoDestroy());
        }

        IEnumerator AutoDestroy()
        {
            yield return new WaitForSeconds(TimeToLiveSecs);
            Destroy(gameObject);
        }

        void FixedUpdate()
        {
            if (!isHit)
            {
                transform.position += transform.right * (projectileStats.Speed / 100f) * Time.deltaTime;
            }
            // Move the projectile in the direction it is facing.
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // If set to  attack player, and trigger enter was not player, return
            if (attackPlayer != other.CompareTag("Player")) return;

            ExplodeAnim();
            
            if (!isHit)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius, LayerMask.GetMask(attackPlayer ? "Player" : "Enemy"));
                foreach (var collider in colliders)
                {
                    collider.GetComponent<EntityBody>()?.Damage(projectileStats.Attack);
                }
            }

            isHit = true;
        }

        public void ExplodeAnim()
        {
            animator.SetTrigger(Explode);
        }

        public void DestroyProjectile()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }
    }
}