using System;
using Level;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    [RequireComponent(typeof(LevelManager))]
    public class EnemySpawnManager : MonoBehaviour
    {
        public LevelManager levelManager;
        [Tooltip("Enemy spawning is divided into sections. This determines how big each section is."),Min(0.1f)]
        public float spawnSectionSize = 10f;

        public float difficultyLevel = 1f;

        /// <summary>
        /// Number of spawn sections in the level.
        /// </summary>
        public int SectionsCount => Mathf.FloorToInt(levelManager.levelSize / spawnSectionSize);
        
        
        public Rect[] GetSpawnSections()
        {
            Vector2 spawnSectionOffset = Vector2.left * (levelManager.levelSize / 2 - spawnSectionSize/2);
            Vector2 gap = (levelManager.levelSize - SectionsCount * spawnSectionSize) / (SectionsCount + 1) * Vector2.left;
            var spawnSections = new Rect[SectionsCount];
            for (int i = 0; i < SectionsCount; i++)
            {
                Vector2 position = (Vector2)transform.position + new Vector2(i * spawnSectionSize, 0) + spawnSectionOffset - gap * (i+1);
                spawnSections[i] = new Rect(position, new Vector2(spawnSectionSize, 1));
            }
            return spawnSections;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            foreach (Rect section in GetSpawnSections())
            {
                Gizmos.DrawWireCube(section.position, new Vector3(spawnSectionSize, 20, 1));
            }
        }
    }
}