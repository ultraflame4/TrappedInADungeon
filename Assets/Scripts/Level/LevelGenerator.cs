using System;
using System.Collections;
using Cinemachine;
using EasyButtons;
using UnityEngine;
using Utils;

namespace Level
{
    /// <summary>
    /// Responsible for the level generation and putting game objects where they need to be.
    /// </summary>
    [RequireComponent(typeof(EnemySpawnManager))]
    public class LevelGenerator : MonoBehaviour
    {
        [Tooltip("The Confiner component of the cinemachine virtual camera to restrict camera movement")]
        public CinemachineConfiner2D cameraConfiner;
        [Tooltip("The player interactable game object place at the start and end of the level to transport the player between areas")]
        public GameObject levelPortalPrefab;
        private ParallaxBackground background;
        private EnemySpawnManager enemySpawnManager;
        private Transform player;
        private PortalInteraction startPortal;
        private PortalInteraction endPortal;

        [field: SerializeField]
        public float levelSize { get; private set; } = 10f;

        [field: SerializeField]
        public float levelHeight { get; private set; } = 30f;

        [field: SerializeField,Tooltip("The y position of the ground in relative to this object")]
        public float groundLevel { get; private set; } = 1f;

        public Vector2 LocalLevelLeft => new Vector2(-levelSize / 2, 0);
        public Vector2 LocalLevelRight => new Vector2(levelSize / 2, 0);
        public Vector2 WorldLevelLeft => (Vector2)transform.position+LocalLevelLeft;
        public Vector2 WorldLevelRight => (Vector2)transform.position+LocalLevelRight;

        private void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
            GenerateLevel();
        }

        /// <summary>
        /// Retrieves the required components for the level manager.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        void GetRequiredComponents()
        {
            background = GetComponentInChildren<ParallaxBackground>();
            enemySpawnManager = GetComponent<EnemySpawnManager>();
            if (background is null) throw new NullReferenceException("Could not find ParallaxBackground component in children of LevelManager");
        }

        [Button]
        public void GenerateLevel()
        {
            GetRequiredComponents();
            background.Generate(levelSize);
            enemySpawnManager.GenerateSpawnSections();
            GenerateColliderBounding();
            if (player != null) // Transport player to start of level. todo change this later to level start
            {
                player.transform.position = LocalLevelLeft + Vector2.right + Vector2.up*groundLevel + Vector2.up;
            }

            if (levelPortalPrefab == null)
            {
                Debug.LogError("The level portal prefab is not assigned!");
                return;
            }

            PlaceLevelPortals(levelPortalPrefab);
        }

        void PlaceLevelPortals(GameObject prefab)
        {
            Transform container = transform.FindOrCreateChild("LevelPortalsCtn",emptyContent:true).transform;
            float placementOffset = 5;
            Vector2 yOffset =
                    Vector2.up * (groundLevel+transform.position.y +
                    prefab.GetComponent<SpriteRenderer>().sprite.bounds.size.y/2);
            startPortal = Instantiate(prefab, LocalLevelLeft + Vector2.right * placementOffset + yOffset,Quaternion.identity,container).GetComponent<PortalInteraction>();
            endPortal = Instantiate(prefab, LocalLevelRight + Vector2.left * placementOffset+ yOffset,Quaternion.identity,container).GetComponent<PortalInteraction>();
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
            poly.points = new[] {
                    new Vector2(LocalLevelLeft.x, levelHeight),
                    new Vector2(LocalLevelLeft.x + levelSize, levelHeight),
                    new Vector2(LocalLevelLeft.x + levelSize, 0),
                    new Vector2(LocalLevelLeft.x, 0),
            };
            edge.points = new[] {
                    new Vector2(LocalLevelLeft.x, levelHeight),
                    new Vector2(LocalLevelLeft.x + levelSize, levelHeight),
                    new Vector2(LocalLevelLeft.x + levelSize, groundLevel),
                    new Vector2(LocalLevelLeft.x, groundLevel),
                    new Vector2(LocalLevelLeft.x, levelHeight)
            };
            StartCoroutine(UpdateConfinerCoroutine());
        }

        IEnumerator UpdateConfinerCoroutine()
        {
            // Wait a while for things to update before updating the camera confine (by invalidating its cache)
            yield return new WaitForSeconds(0.1f);
            cameraConfiner.InvalidateCache();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow + Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(0, groundLevel), new Vector3(levelSize, 0));
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + new Vector3(0, levelHeight / 2), new Vector3(levelSize, levelHeight));
        }
    }
}