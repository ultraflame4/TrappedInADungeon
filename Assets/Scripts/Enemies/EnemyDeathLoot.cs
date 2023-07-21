using System;
using Drops;
using Entities;
using Level;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EntityBody))]
    public class EnemyDeathLoot : MonoBehaviour
    {
        private EntityBody enemyBody;
        public GameObject expBallPrefab;
        public SpawnableEnemy config;
        
        private const float ExperiencePointsPerBall = 30;
        private const int maxBalls = 20;
        
        private void Start()
        {
            enemyBody = GetComponent<EntityBody>();
            enemyBody.DeathEvent += OnDeath;
        }

        void OnDeath()
        {
            int expDropped = config.difficultyPoints * (enemyBody.Level + 1) / 2;
            int ballsCount = Math.Min(maxBalls, Mathf.CeilToInt(expDropped / ExperiencePointsPerBall));
            float expPerBall = expDropped / ballsCount;
            Instantiate(expBallPrefab, transform.position, Quaternion.identity).GetComponent<ExpBall>().expValue = expPerBall;
        }
    }
}