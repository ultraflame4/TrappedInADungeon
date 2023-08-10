using Core.Entities;
using Core.Utils;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level
{
    /// <summary>
    /// Spawns enemies configured on start.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Tooltip("The width of the area to spawn the enemy in")]
        public float spawnRect = 5f;

        [Tooltip("Level of enemy to spawn")]
        public int enemySpawnLevel = 1;
        /// <summary>
        /// Variation in enemy level.
        /// </summary>
        [HideInInspector]
        public ValueRange<int> enemySpawnLevelRange = new (0,0);
        [Tooltip("Enemies to spawn")]
        public SpawnableEnemy[] enemies;

        void Start()
        {
            Spawn(); // Spawn enemies
        }
        
        void Spawn()
        {
            // If enemy spawning is disabled, skip
            if (!GameManager.Instance.SpawnEnemies) return;
            // Iterate through all the enemies to spawn and spawn them.
            for (var i = 0; i < enemies.Length; i++)
            {
                // Calculate the local x offset to spawn them at (to equally distribute them throughout the spawnRect)
                float x = i * (spawnRect / enemies.Length) - spawnRect / 2f;
                // Instantiate the enemy with the x offset
                EntityBody body = Instantiate(enemies[i].enemyPrefab, new Vector3(transform.position.x+x, transform.position.y), Quaternion.identity)
                        .GetComponent<EntityBody>();
                // Configure the spawned enemy lootbox
                body.GetComponent<EnemyDeathLoot>()?.SetConfig(enemies[i]);
                // Set the enemy level + variation range
                body.Level = enemySpawnLevel + Mathf.RoundToInt(Random.Range(enemySpawnLevelRange.min,enemySpawnLevelRange.max));
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawCube(transform.position, new Vector3(spawnRect, 1));
        }
    }
}