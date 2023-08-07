using Core.Enemies;
using Core.Entities;
using Enemies.Behaviours.Follow;
using UnityEngine;

namespace Enemies.Behaviours.Attacks
{
    /// <summary>
    /// Ranged attack for enemies. The attacks are triggered by animation events. Call Shoot() from the animation event.
    /// </summary>
    [RequireComponent(typeof(EntityBody), typeof(EnemyFollow))]
    public class EnemyRangedAttack : EnemyStateBehaviour
    {
        // Component references
        private EntityBody entityBody;
        private EnemyFollow follow;

        [Tooltip("Projectile prefab to spawn.")]
        public GameObject projectilePrefab;

        [Tooltip("Projectile speed.")]
        public float projSpeed = 500f;

        private void Start()
        {
            // Get component references
            follow = GetComponent<EnemyFollow>();
            entityBody = GetComponent<EntityBody>();
        }

        public override void StateEnter()
        {
            // Start attack animation when state enters
            stateManager.SetAttackAnim(true);
        }

        public override void StateExit()
        {
            // End attack animation when state exits
            stateManager.SetAttackAnim(false);
        }

        private void FixedUpdate()
        {
            // Don't do anything if state isn't active
            if (!stateActive) return;
            // If player isn't with attack range, transition to alert state
            if (!follow.CheckPlayerWithinAttackRange()) stateManager.TransitionState(EnemyStates.ALERT);
        }

        /// <summary>
        /// This method is called from an animation event.
        /// </summary>
        public void Shoot()
        {
            // Spawn projectile and get the Projectile component
            var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<Projectile.Projectile>();
            if (!projectile)
            {
                Debug.LogWarning($"Could not get Projectile component on prefab {projectilePrefab.name}!");
            }
            // Copy entity stats to use for projectile
            var stats = SEntityStats.Create(entityBody);
            // Set projectile speed (shouldn't use the one in entityBody because it will be too fast)
            stats.Speed = projSpeed;
            projectile.projectileStats = stats; // Set projectile stats
        }
    }
}