using System;
using EasyButtons;
using Level;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Random = UnityEngine.Random;

namespace Enemies
{
    [RequireComponent(typeof(LevelManager))]
    public class EnemySpawnManager : MonoBehaviour
    {
        public LevelManager levelManager;

        [Tooltip("Enemy spawning is divided into sections. This determines how big each section is."), Min(0.1f)]
        public float spawnSectionSize = 10f;
        public float yOffSet = 2f;

        public float difficultyLevel = 1f;

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
                sectionObj.transform.position = section + randomPosition + Vector2.up * yOffSet;
                sectionObj.AddComponent<EnemySpawner>();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            foreach (Vector2 section in GetSpawnSectionPositions())
            {
                Gizmos.DrawWireCube(section+Vector2.up*yOffSet, new Vector3(spawnSectionSize, 1, 1));
            }
        }
    }
}