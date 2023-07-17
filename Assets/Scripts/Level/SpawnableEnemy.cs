using UnityEngine;
using UnityEngine.Serialization;

namespace Level
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
        [Tooltip("Minimum player level required to spawn this enemy.")]
        public int minPlayerLevel = 0;
        
    }
}