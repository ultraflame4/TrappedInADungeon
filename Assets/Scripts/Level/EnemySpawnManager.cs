using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using Enemies;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Random = UnityEngine.Random;

namespace Level
{
    [RequireComponent(typeof(LevelGenerator))]
    public class EnemySpawnManager : MonoBehaviour
    {
        public LevelGenerator levelGenerator;
        public PlayerBody player;

        [Tooltip("Enemy spawning is divided into sections. This determines how big each section is."), Min(0.1f)]
        public float spawnSectionSize = 10f;

        [FormerlySerializedAs("yOffset")]
        public float _yOffset = 2f;

        [Tooltip("Special zone at the ends of the level where enemies will not spawn")]
        public float endsOffset = 20f;

        private float yOffset => _yOffset + levelGenerator.groundLevel;

        public SpawnableEnemy[] enemyPool;
        public int difficultyPoints = 200;

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

        public GameObject[] ChooseEnemiesFromPool(int allocatedPoints)
        {
            List<GameObject> enemyPrefabs = new();

            int pointsRemaining = allocatedPoints;
            var sortedEnemyPool = enemyPool.OrderBy(x => x.spawnWeight).ToList();
            while (pointsRemaining > 0)
            {
                SpawnableEnemy chosen = GetRandomEnemy(pointsRemaining, sortedEnemyPool);
                if (chosen is null) break;

                pointsRemaining -= chosen.difficultyPoints;
                enemyPrefabs.Add(chosen.enemyPrefab);
            }

            return enemyPrefabs.ToArray();
        }

        /// <summary>
        /// Gets a random enemy from the pool, taking into account the player's level, points available & spawn chances weights.
        /// </summary>
        /// <param name="pointsAvailable">Difficulty points available</param>
        /// <param name="sortedEnemyPool">The sorted enemy pool</param>
        /// <returns>Returns the chosen enemy or null if no enemies can be spawned</returns>
        private SpawnableEnemy GetRandomEnemy(int pointsAvailable, List<SpawnableEnemy> sortedEnemyPool)
        {
            SpawnableEnemy[] filteredEnemyPool = sortedEnemyPool
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
            int weightSum = filteredEnemyPool.Sum(x => x.spawnWeight); // Sum of spawnWeights
            int positionCounter = 0;
            Dictionary<int, SpawnableEnemy> positions = new();
            for (var i = 0; i < filteredEnemyPool.Length; i++)
            {
                var enemy = filteredEnemyPool[i];
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
                spawner.enemyPrefabs = ChooseEnemiesFromPool(difficultyPoints / SectionsCount);
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