using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "SpawnableEnemy", menuName = "GameContent/SpawnableEnemy", order = 0)]
    public class SpawnableEnemy : ScriptableObject
    {
        [Tooltip("Enemy to spawn")]
        public GameObject enemyPrefab;
        [Tooltip("Chance of this enemy spawning")]
        public float spawnChance = 1f;
        [Tooltip("How many points this enemy is worth in terms of difficulty.")]
        public int diffucultyPoints = 1;
    }
}