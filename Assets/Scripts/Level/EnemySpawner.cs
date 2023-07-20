using Enemies;
using UnityEngine;

namespace Level
{
    //todo rework enemy spawning system
    public class EnemySpawner : MonoBehaviour
    {
        public float spawnRect = 5f;
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
                var obj = Instantiate(enemyPrefabs[i], new Vector3(transform.position.x+x, transform.position.y), Quaternion.identity);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red * 0.5f;
            Gizmos.DrawCube(transform.position, new Vector3(spawnRect, 1));
        }
    }
}