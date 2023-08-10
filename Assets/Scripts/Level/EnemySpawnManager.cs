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
        // Component references
        [Tooltip("LevelGenerator to use")]
        public LevelGenerator levelGenerator;

        [Tooltip("The stats of the player")]
        public PlayerBody player;

        [Tooltip("Level of enemies to spawn. Overriden by LevelGenerator")]
        public int EnemyLevel = 1;

        [Tooltip("Addional range of enemy levels to vary the enemy levels")]
        public int EnemyLevelRangeMin = -1;

        public int EnemyLevelRangeMax = 5;

        [Tooltip("Enemy spawning is divided into sections. This determines how big each section is."), Min(0.1f)]
        public float spawnSectionSize = 10f;

        [Tooltip("y offset from the ground to spawn the enemies at")]
        public float _yOffset = 2f;

        [Tooltip("Special zone at the ends of the level where enemies will not spawn")]
        public float endsOffset = 20f;

        /// <summary>
        /// Total yOffset to use when spawning enemies
        /// </summary>
        private float yOffset => _yOffset + levelGenerator.groundLevel;

        [Tooltip("Spawnable enemies")]
        public SpawnableEnemy[] enemyPool;


        [Tooltip("Number of enemies Per Section ")]
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
            // Generate the sections to spawn the enemies
            GenerateSpawnSections();
        }

        /// <summary>
        /// Calculate and return the position of the enemy spawn sections
        /// </summary>
        private Vector2[] GetSpawnSectionPositions()
        {
            // How much to offset the spawnSections so that they are centered
            Vector2 spawnSectionOffset = Vector2.left * (levelGenerator.levelSize / 2 - spawnSectionSize / 2);
            // The gap offset for the in between sections so that the spawnSections are distributed uniformly
            Vector2 gap = (SpawnableAreaSize - SectionsCount * spawnSectionSize) / (SectionsCount + 1) * Vector2.left;
            // New position of spawn sections
            var spawnSections = new Vector2[SectionsCount];
            // Special offset to ensure the areas near the start and end of the level is clear of enemies.A
            Vector2 spawnableOffset = Vector2.right * endsOffset;
            // Calculate the final position (after adding all the funny offsets) of the spawnSection
            for (int i = 0; i < SectionsCount; i++)
            {
                spawnSections[i] = (Vector2)transform.position + spawnableOffset + new Vector2(i * spawnSectionSize, 0) + spawnSectionOffset - gap * (i + 1);
            }

            // In summary: a lot of position offsetting here such that the spawn sections are uniformly distributed without touching the ends of the level. 
            return spawnSections;
        }

        /// <summary>
        /// Chooses a set of enemies to spawn from the the spawnable enemy pool based on the difficulty points available
        /// </summary>
        /// <param name="allocatedPoints">Amount of difficulty points available</param>
        /// <returns></returns>
        public SpawnableEnemy[] ChooseEnemiesFromPool(int allocatedPoints)
        {
            List<SpawnableEnemy> enemies = new(); // list to store enemies

            int pointsRemaining = allocatedPoints;
            // Randomised the amount of enemies to spawn.
            // Is max enemies because if the difficulty points run out, the no. of enemy chosen will not match the no. of enemies to spawn.
            int maxEnemyCount = Mathf.RoundToInt(Random.Range(MaxEnemyCount.min, MaxEnemyCount.max));

            // How many enemies can be spawned
            int enemyCountLeft = maxEnemyCount;
            // Keep choosing enemies until points run out
            while (pointsRemaining > 0)
            {
                // Get a random enemy from the pool
                SpawnableEnemy chosen = GetRandomEnemy(pointsRemaining, 1f - enemyCountLeft / (float)maxEnemyCount, enemyPool);
                // If no enemy has been chosen due to reasons, break;
                if (chosen is null) break;
                // point cost of chosen enemy
                pointsRemaining -= chosen.difficultyPoints;
                // Add to list
                enemies.Add(chosen);
                // Reduce amt of enemies avail to spawn
                enemyCountLeft--;
                // If amt enemies to spawn is 0, quit 
                if (enemyCountLeft == 0) break;
            }

            // convert to array & return
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
            // Filter and get the eligible enemies in the enemy pool
            SpawnableEnemy[] filteredEnemyPool = enemyPool
                    // Select eligible enemies.
                    .Where(x => x.difficultyPoints <= pointsAvailable && x.minPlayerLevel <= player.Level)
                    .ToArray(); // Filter out invalid enemies

            // If no eligible enemies return null
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
                    .OrderBy(x => x.spawnWeight) // Order enemies in ascending order. Very important. Explanation is the big comment above
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
        

        /// <summary>
        /// Generates and Instantiates the various spawn sections (the EnemySpawner)
        /// </summary>
        [Button]
        public void GenerateSpawnSections()
        {
            // Find the container object to spawn the enemy spawner in
            var container = GameObject.Find("SpawnSections");
            // If cannot find container, create a new one
            if (container is null)
            {
                container = new GameObject("SpawnSections");
                container.transform.SetParent(transform);
            }
            // Ensure that the container does not have any spawn sections
            container.DestroyChildren();
            // Loop through the spawn section positions and create the enemy spawner for each spawn section 
            foreach (Vector2 section in GetSpawnSectionPositions())
            {
                // Have a slight random x offset added to the position so that it doesnt look awkward.
                Vector2 randomPosition = Vector2.right * (spawnSectionSize / 4 * Random.Range(-1, 1));
                // Create a new game object
                var sectionObj = new GameObject("SpawnSection");
                // Set the parent
                sectionObj.transform.SetParent(container.transform);
                // Set the position with the random x offset applied
                sectionObj.transform.position = section + randomPosition + Vector2.up * yOffset;
                // Add the enemy spawner component
                var spawner = sectionObj.AddComponent<EnemySpawner>();
                // Configure the enemy spawner
                spawner.spawnRect = spawnSectionSize / 2;
                spawner.enemySpawnLevel = EnemyLevel;
                spawner.enemySpawnLevelRange.min = EnemyLevelRangeMin;
                spawner.enemySpawnLevelRange.max = EnemyLevelRangeMax;
                // Choose the enemies
                spawner.enemies = ChooseEnemiesFromPool(difficultyPoints / SectionsCount);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            // Loop through the spawn sections and draw a rect to show the rough location of the spawn areas. 
            foreach (Vector2 section in GetSpawnSectionPositions())
            {
                Gizmos.DrawWireCube(section + Vector2.up * yOffset, new Vector3(spawnSectionSize, 1, 1));
            }
        }
    }
}