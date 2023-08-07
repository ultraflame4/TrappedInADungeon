using System;
using Core.Entities;
using Core.Item;
using Loot;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    [RequireComponent(typeof(EntityBody))]
    public class EnemyDeathLoot : MonoBehaviour
    {
        // --- References to other scripts & prefabs---
        private EntityBody enemyBody;

        [Tooltip("Exp ball prefab to spawn when the enemy dies")]
        public GameObject expBallPrefab;

        [Tooltip("dropped item prefab to spawn when the enemy dies")]
        public GameObject droppedItemPrefab;
        private SpawnableEnemy config;

        // --- Constants ---
        private const float ExperiencePointsPerBall = 30;
        private const int maxBalls = 20;

        private void Start()
        {
            enemyBody = GetComponent<EntityBody>();
            enemyBody.DeathEvent += OnDeath;
        }

        /// <summary>
        /// Sets the enemy configuration for this enemy
        /// </summary>
        /// <param name="config"></param>
        public void SetConfig(SpawnableEnemy config)
        {
            this.config = config;
        }

        void OnDeath()
        {
            // If the enemy has no config, don't drop anything
            if (config == null) return;
            // Calculate the amount of experience balls to drop
            int expDropped = config.difficultyPoints * (enemyBody.Level + 1) / 2;
            // Calculate number of balls to drop
            int ballsCount = Math.Min(maxBalls, Mathf.CeilToInt(expDropped / ExperiencePointsPerBall));
            // Calculate the experience value of each ball
            float expPerBall = expDropped / (ballsCount + 1);
            // Instantiate the exp balls
            Instantiate(expBallPrefab, transform.position, Quaternion.identity).GetComponent<ExpBall>().expValue = expPerBall;

            // Loop through all lootbox items in the enemy config
            foreach (var lootboxItem in config.lootbox)
            {
                // Randomly decide whether to spawn the item based on the chance
                bool toSpawn = Random.value <= lootboxItem.chance;
                if (!toSpawn) continue; // If not, skip this item
                // Spawn the loot item prefab
                DroppedLootItem lootItem = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity).GetComponent<DroppedLootItem>();
                // Tell the loot item prefab what item it is.
                lootItem.SetItem(new ItemInstance(lootboxItem.item));
            }
        }
    }
}