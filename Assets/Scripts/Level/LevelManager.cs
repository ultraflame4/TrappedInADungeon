using System;
using System.Collections;
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
        
        [field: SerializeField]
        public float levelHeight { get; private set; }  = 30f;
        public Vector2 LevelLeft => new Vector2(-levelSize/2, 0);
        
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
        /// <summary>
        /// Generates the bounding for the camera confiner (which uses a polygon collider 2D as bounding).
        /// Also generates the edge colliders to block player.
        /// </summary>
        void GenerateColliderBounding()
        {
            PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
            EdgeCollider2D edge = GetComponent<EdgeCollider2D>();
            if (poly is null)
            {
                poly = gameObject.AddComponent<PolygonCollider2D>();
                edge = gameObject.AddComponent<EdgeCollider2D>();
            }

            poly.enabled = false;
            edge.enabled = true;
            poly.points =  new[] {
                    new Vector2(LevelLeft.x, levelHeight),
                    new Vector2(LevelLeft.x + levelSize, levelHeight),
                    new Vector2(LevelLeft.x + levelSize, 0),
                    new Vector2(LevelLeft.x, 0),
            };
            edge.points = new[] {
                    new Vector2(LevelLeft.x, levelHeight),
                    new Vector2(LevelLeft.x + levelSize, levelHeight),
                    new Vector2(LevelLeft.x + levelSize, 0),
                    new Vector2(LevelLeft.x, 0),
                    new Vector2(LevelLeft.x, levelHeight)
            };
            StartCoroutine(UpdateConfinerCoroutine());
        }

        IEnumerator UpdateConfinerCoroutine()
        {
            // Wait a while for things to update before updating the camera confine (by invalidating its cache)
            yield return new WaitForSeconds(0.1f);
            cameraConfiner.InvalidateCache();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position+ new Vector3(0, levelHeight/2), new Vector3(levelSize, levelHeight, 1));
            
        }
    }
}