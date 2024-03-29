﻿using System;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "SpawnableEnemy", menuName = "GameContent/SpawnableEnemy", order = 0)]
    public class SpawnableEnemy : ScriptableObject
    {
        [Tooltip("Enemy to spawn")]
        public GameObject enemyPrefab;
        [Tooltip("Chance of this enemy spawning"),Min(1)]
        public int spawnWeight = 1;
        [Tooltip("How many points this enemy is worth in terms of difficulty.")]
        public int difficultyPoints = 1;
        [Tooltip("Minimum player level required to spawn this enemy."), Min(0)]
        public int minPlayerLevel = 0;
        [Tooltip("Enemy lootbox configuration - What it will drop on death")]
        public LootboxItem[] lootbox;
    }
}