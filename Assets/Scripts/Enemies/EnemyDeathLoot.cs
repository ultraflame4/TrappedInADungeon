﻿using System;
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
        private EntityBody enemyBody;
        public GameObject expBallPrefab;
        public GameObject droppedItemPrefab;
        private SpawnableEnemy config;

        private const float ExperiencePointsPerBall = 30;
        private const int maxBalls = 20;

        private void Start()
        {
            enemyBody = GetComponent<EntityBody>();
            enemyBody.DeathEvent += OnDeath;
        }

        public void SetConfig(SpawnableEnemy config)
        {
            this.config = config;
        }

        void OnDeath()
        {
            int expDropped = config.difficultyPoints * (enemyBody.Level + 1) / 2;
            int ballsCount = Math.Min(maxBalls, Mathf.CeilToInt(expDropped / ExperiencePointsPerBall));
            float expPerBall = expDropped / (ballsCount + 1);
            Instantiate(expBallPrefab, transform.position, Quaternion.identity).GetComponent<ExpBall>().expValue = expPerBall;

            foreach (var lootboxItem in config.lootbox)
            {
                // Whether the lootbox item should be dropped
                bool toSpawn = Random.value <= lootboxItem.chance;
                if (!toSpawn) continue;
                DroppedLootItem lootItem = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity).GetComponent<DroppedLootItem>();
                lootItem.SetItem(new ItemInstance(lootboxItem.item));
            }
        }
    }
}