using EasyButtons;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Random = UnityEngine.Random;

namespace Level
{
    [RequireComponent(typeof(LevelManager))]
    public class EnemySpawnManager : MonoBehaviour
    {
        public LevelManager levelManager;

        [Tooltip("Enemy spawning is divided into sections. This determines how big each section is."), Min(0.1f)]
        public float spawnSectionSize = 10f;
        [FormerlySerializedAs("yOffSet")]
        public float yOffset = 2f;
        public SpawnableEnemy[] enemyPool;
        public int difficultyPoints = 200;

        /// <summary>
        /// Number of spawn sections in the level.
        /// </summary>
        public int SectionsCount => Mathf.FloorToInt(levelManager.levelSize / spawnSectionSize);

        private void Start()
        {
            GenerateSpawnSections();
        }

        private Vector2[] GetSpawnSectionPositions()
        {
            Vector2 spawnSectionOffset = Vector2.left * (levelManager.levelSize / 2 - spawnSectionSize / 2);
            Vector2 gap = (levelManager.levelSize - SectionsCount * spawnSectionSize) / (SectionsCount + 1) * Vector2.left;
            var spawnSections = new Vector2[SectionsCount];
            for (int i = 0; i < SectionsCount; i++)
            {
                spawnSections[i] = (Vector2)transform.position + new Vector2(i * spawnSectionSize, 0) + spawnSectionOffset - gap * (i + 1);
            }

            return spawnSections;
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
                spawner.allocatedDifficultyPoints = difficultyPoints / SectionsCount;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            foreach (Vector2 section in GetSpawnSectionPositions())
            {
                Gizmos.DrawWireCube(section+Vector2.up*yOffset, new Vector3(spawnSectionSize, 1, 1));
            }
        }
    }
}