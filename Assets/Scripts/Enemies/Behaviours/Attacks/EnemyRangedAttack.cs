using Core.Enemies;
using Core.Entities;
using Enemies.Behaviours.Follow;
using UnityEngine;

namespace Enemies.Behaviours.Attacks
{
    /// <summary>
    /// ranged attack for enemies. The attacks are triggered by animation events. Call Shoot() from the animation event.
    /// </summary>
    [RequireComponent(typeof(EntityBody), typeof(EnemyFollow))]
    public class EnemyRangedAttack  : EnemyStateBehaviour
    {
        private EntityBody entityBody;
        private EnemyFollow follow;
        [Tooltip("Projectile prefab to spawn.")]
        public GameObject projectilePrefab;
        [Tooltip("Projectile speed.")]
        public float projSpeed = 500f;
        private void Start()
        {
            follow = GetComponent<EnemyFollow>();
            entityBody = GetComponent<EntityBody>();
        }
        public override void StateEnter()
        {
            stateManager.SetAttackAnim(true);
        }
        public override void StateExit()
        {
            stateManager.SetAttackAnim(false);
        }
        
        private void FixedUpdate()
        {
            if (!stateActive) return;
            if (!follow.CheckPlayerWithinAttackRange())
            {
                stateManager.TransitionState(EnemyStates.ALERT);
            }
        }

        public void Shoot()
        {
            var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<Projectile.Projectile>();
            if (!projectile)
            {
                Debug.LogWarning($"Could not get Projectile component on prefab {projectilePrefab.name}!");
            }

            var stats = SEntityStats.Create(entityBody);
            stats.Speed = projSpeed;
            projectile.projectileStats = stats;
        }
    }
}