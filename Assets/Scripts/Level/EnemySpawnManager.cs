using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using EasyButtons;
using Enemies;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Level
{
    [RequireComponent(typeof(LevelGenerator))]
    public class EnemySpawnManager : MonoBehaviour
    {
        public LevelGenerator levelGenerator;
        public PlayerBody player;

        [Tooltip("Level of enemies to spawn. Overriden by LevelGenerator")]
        public int EnemyLevel = 1;

        [Tooltip("Addional range of enemy levels to vary the enemy levels")]
        public int EnemyLevelRangeMin = -1;

        public int EnemyLevelRangeMax = 5;

        [Tooltip("Enemy spawning is divided into sections. This determines how big each section is."), Min(0.1f)]
        public float spawnSectionSize = 10f;

        [FormerlySerializedAs("yOffset")]
        public float _yOffset = 2f;

        [Tooltip("Special zone at the ends of the level where enemies will not spawn")]
        public float endsOffset = 20f;

        private float yOffset => _yOffset + levelGenerator.groundLevel;

        public SpawnableEnemy[] enemyPool;

        [FormerlySerializedAs("EnemyCount"), Tooltip("Enemy Count Range Per Section ")]
        public ValueRange<float> MaxEnemyCount = new(8, 25);

        [Tooltip("General difficulty of the spawned enemies.")]
        public int startDifficultyPoints = 70;

        [Tooltip("General difficulty increase per player level")]
        public int difficultyPointIncreasePerLevel = 15;

        public int difficultyPoints => startDifficultyPoints + difficultyPointIncreasePerLevel * (player.Level - 1);

        /// <summary>
        /// The size of the area where enemies are allowed to spawn, aka levelSize - endsOffset * 2
        /// </summary>
        private float SpawnableAreaSize => levelGenerator.levelSize - endsOffset * 2;

        /// <summary>
        /// Number of spawn sections in the level.
        /// </summary>
        public int SectionsCount => Mathf.FloorToInt(SpawnableAreaSize / spawnSectionSize);

        private void Start()
        {
            GenerateSpawnSections();
        }

        private Vector2[] GetSpawnSectionPositions()
        {
            Vector2 spawnSectionOffset = Vector2.left * (levelGenerator.levelSize / 2 - spawnSectionSize / 2);
            Vector2 gap = (SpawnableAreaSize - SectionsCount * spawnSectionSize) / (SectionsCount + 1) * Vector2.left;
            var spawnSections = new Vector2[SectionsCount];
            Vector2 spawnableOffset = Vector2.right * endsOffset;
            for (int i = 0; i < SectionsCount; i++)
            {
                spawnSections[i] = (Vector2)transform.position + spawnableOffset + new Vector2(i * spawnSectionSize, 0) + spawnSectionOffset - gap * (i + 1);
            }

            return spawnSections;
        }

        public SpawnableEnemy[] ChooseEnemiesFromPool(int allocatedPoints)
        {
            List<SpawnableEnemy> enemies = new();

            int pointsRemaining = allocatedPoints;
            int maxEnemyCount = Mathf.RoundToInt(Random.Range(MaxEnemyCount.min, MaxEnemyCount.max));

            int enemyCountLeft = maxEnemyCount;
            while (pointsRemaining > 0)
            {
                SpawnableEnemy chosen = GetRandomEnemy(pointsRemaining, 1f - enemyCountLeft / (float)maxEnemyCount, enemyPool);
                if (chosen is null) break;

                pointsRemaining -= chosen.difficultyPoints;
                enemies.Add(chosen);
                enemyCountLeft--;
                if (enemyCountLeft == 0) break;
            }

            return enemies.ToArray();
        }

        /// <summary>
        /// Gets a random enemy from the pool, taking into account the player's level, points available & spawn chances weights.
        /// </summary>
        /// <param name="pointsAvailable">Difficulty points available</param>
        /// <param name="dSkew">How much to skew towards the stronger enemies (0 is easy, 1 is strong)</param>
        /// <param name="sortedEnemyPool">The sorted enemy pool</param>
        /// <returns>Returns the chosen enemy or null if no enemies can be spawned</returns>
        private SpawnableEnemy GetRandomEnemy(int pointsAvailable, float dSkew, SpawnableEnemy[] enemyPool)
        {
            SpawnableEnemy[] filteredEnemyPool = enemyPool
                    .Where(x => x.difficultyPoints <= pointsAvailable && x.minPlayerLevel <= player.Level)
                    .ToArray(); // Filter out invalid enemies

            if (filteredEnemyPool.Length == 0) return null;

            /*
             * The spawnWeight of each enemy is used to determine the probability of it spawning.
             * If 2 enemies have spawnWeights of 1 and 2, then the first enemy has a 1/3 chance of spawning, and the second has a 2/3 chance.
             *
             * To calculate this we essentially draw a graph
             * |--1--|----2----| - model
             * The model above represents the chances of the 2 enemies spawning.
             * We can then add positions
             * |--1--|----2----| - model
             * 0     1         3
             * The bigger the weight, the wider the section, and hence the position where the section ends is also bigger.
             * We can then generate random position from 0 to the sum of all weights - which is the end position of the model.
             * And if the random position is < end position of 1 section, we know that the position is in that section and hence choose that section as outcome.
             * We can do this because the weight of each enemy (and hence size of each section) is sorted in ascending order.
             */
            int weightSum = filteredEnemyPool.Sum(x => x.spawnWeight); // Initial Sum of spawnWeights
            SpawnableEnemy[] skewedPool = filteredEnemyPool.Select(x =>
                    {
                        var copy = Instantiate(x);
                        // Fancy maths to skew spawn weight
                        copy.spawnWeight = Mathf.RoundToInt(copy.spawnWeight + copy.difficultyPoints * dSkew * weightSum);
                        return copy;
                    })
                    .OrderBy(x => x.spawnWeight)
                    .ToArray();
             weightSum = skewedPool.Sum(x => x.spawnWeight); // Re calculate sum of spawnWeights
            int positionCounter = 0;
            Dictionary<int, SpawnableEnemy> positions = new();
            for (var i = 0; i < skewedPool.Length; i++)
            {
                var enemy = skewedPool[i];
                positionCounter += enemy.spawnWeight;
                positions.Add(positionCounter, enemy);
            }

            int randomPosition = Random.Range(0, weightSum);
            return positions.First(x => randomPosition < x.Key).Value;
        }

        [Button]
        public void GenerateSpawnSections()
        {
            var container = GameObject.Find("SpawnSections");
            if (container is null)
            {
                container = new GameObject("SpawnSections");
                container.transform.SetParent(transform);
            }

            container.DestroyChildren();
            foreach (Vector2 section in GetSpawnSectionPositions())
            {
                Vector2 randomPosition = Vector2.right * (spawnSectionSize / 4 * Random.Range(-1, 1));
                var sectionObj = new GameObject("SpawnSection");
                sectionObj.transform.SetParent(container.transform);
                sectionObj.transform.position = section + randomPosition + Vector2.up * yOffset;
                var spawner = sectionObj.AddComponent<EnemySpawner>();
                spawner.spawnRect = spawnSectionSize/2;
                spawner.enemySpawnLevel = EnemyLevel;
                spawner.enemySpawnLevelRangeMin = EnemyLevelRangeMin;
                spawner.enemySpawnLevelRangeMax = EnemyLevelRangeMax;
                spawner.enemies = ChooseEnemiesFromPool(difficultyPoints / SectionsCount);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            foreach (Vector2 section in GetSpawnSectionPositions())
            {
                Gizmos.DrawWireCube(section + Vector2.up * yOffset, new Vector3(spawnSectionSize, 1, 1));
            }
        }
    }
}