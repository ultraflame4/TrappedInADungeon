using Core.Entities;
using UnityEngine;

namespace Level
{
    public class EnemySpawner : MonoBehaviour
    {
        public float spawnRect = 5f;
        public int enemySpawnLevel = 1;
        [HideInInspector]
        public int enemySpawnLevelRangeMin = 0;
        [HideInInspector]
        public int enemySpawnLevelRangeMax = 0;
        [Tooltip("Enemies to spawn")]
        public GameObject[] enemyPrefabs;

        void Start()
        {
            Spawn();
        }
        
        void Spawn()
        {
            if (!GameManager.Instance.SpawnEnemies) return;
            for (var i = 0; i < enemyPrefabs.Length; i++)
            {
                float x = i * (spawnRect / enemyPrefabs.Length) - spawnRect / 2f;
                EntityBody body = Instantiate(enemyPrefabs[i], new Vector3(transform.position.x+x, transform.position.y), Quaternion.identity)
                        .GetComponent<EntityBody>();
                body.Level = enemySpawnLevel + Mathf.RoundToInt(Random.Range(enemySpawnLevelRangeMin,enemySpawnLevelRangeMax));
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawCube(transform.position, new Vector3(spawnRect, 1));
        }
    }
}