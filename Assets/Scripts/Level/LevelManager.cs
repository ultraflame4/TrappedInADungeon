using System;
using Cinemachine;
using EasyButtons;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(EnemySpawnManager))]
    public class LevelManager : MonoBehaviour
    {
        public CinemachineConfiner2D cameraConfiner;
        private Ground ground;
        private ParallaxBackground background;
        private EnemySpawnManager enemySpawnManager; 

        [field: SerializeField]
        public float levelSize { get; private set; } = 10f;
        public Vector2 LevelLeft => new Vector2(-levelSize/2, 0);

        private const float levelHeight = 30f;
        

        private void Start()
        {
            GenerateLevel();
        }
        
        /// <summary>
        /// Retrieves the required components for the level manager.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        void GetRequiredComponents()
        {
            background = GetComponentInChildren<ParallaxBackground>();
            ground = GetComponentInChildren<Ground>();
            enemySpawnManager = GetComponent<EnemySpawnManager>();
            if (background is null) throw new NullReferenceException("Could not find ParallaxBackground component in children of LevelManager");
            if (ground is null) throw new NullReferenceException("Could not find Ground component in children of LevelManager");
        }
        
        [Button]
        public void GenerateLevel()
        {
            GetRequiredComponents();
            background.sections = Mathf.CeilToInt(levelSize / background.SectionWidth) + 1;
            background.xOffset = -(background.TotalWidth-background.SectionWidth) / 2;
            background.GenerateLayers();
            ground.UpdateWidth(levelSize);
            enemySpawnManager.GenerateSpawnSections();
            GenerateColliderBounding();
        }

        void GenerateColliderBounding()
        {
            PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
            if (collider is null)
            {
                collider = gameObject.AddComponent<PolygonCollider2D>();
            }

            collider.enabled = false;
            collider.points = new[] {
                    new Vector2(LevelLeft.x, 20),
                    new Vector2(LevelLeft.x + levelSize, 20),
                    new Vector2(LevelLeft.x + levelSize, -20),
                    new Vector2(LevelLeft.x, -20),
            };
            cameraConfiner.InvalidateCache();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(levelSize, levelHeight, 1));
            
        }
    }
}