using System;
using System.Collections;
using Cinemachine;
using Core.UI;
using Core.Utils;
using EasyButtons;
using PlayerScripts;
using UnityEngine;

namespace Level
{
    /// <summary>
    /// Responsible for the level generation and putting game objects where they need to be.
    /// </summary>
    public class LevelGenerator : MonoBehaviour
    {
        [Tooltip("The Confiner component of the cinemachine virtual camera to restrict camera movement")]
        public CinemachineConfiner2D cameraConfiner;

        [Tooltip("The player interactable game object place at the start and end of the level to transport the player between areas")]
        public GameObject levelPortalPrefab;

        [Tooltip("The area index, enemy level scales of this.")]
        public int AreaIndex = 0;

        // References to other game objects
        private ParallaxBackground background;
        private EnemySpawnManager enemySpawnManager;
        private PortalInteraction startPortal;
        private PortalInteraction endPortal;

        
        [field: SerializeField,Tooltip("Size of level (width")]
        public float levelSize { get; private set; } = 10f;

        [field: SerializeField, Tooltip("Height of level")]
        public float levelHeight { get; private set; } = 30f;

        [field: SerializeField, Tooltip("The y position of the ground in relative to this object")]
        public float groundLevel { get; private set; } = 1f;

        
        public Vector2 LocalLevelLeft => new Vector2(-levelSize / 2, 0); // Left side of the level relative to the level position
        public Vector2 LocalLevelRight => new Vector2(levelSize / 2, 0); // Right side of the level relative to the level position

        private void Awake()
        {
            // Register event
            GameManager.Instance.GenerateLevelEvent += BeginLevelGeneration;
        }
        
        /// <summary>
        /// Called when GameManager is ready to generate the level.
        /// </summary>
        void BeginLevelGeneration()
        {
            // The current index of the area we should use
            AreaIndex = GameManager.CurrentAreaIndex;
            GenerateLevel(); // Generate the level
            // Push the notification
            NotificationManager.Instance.PushNotification($"<size=150%>Entered Area {AreaIndex}</size>");
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
            // Generate the background
            background.Generate(levelSize);
            // If enemy spawn manager exists,  
            if (enemySpawnManager != null)
            {
                // configure enemy level for this area
                enemySpawnManager.EnemyLevel = AreaIndex * 5 + 1; // arbitrary equation for enemy level scaling.
                // Tell it to generate the spawn sections
                enemySpawnManager.GenerateSpawnSections();
            }
            // Generate the collider bounding for the cinemachine camera confiner
            // Also generates the level collider for the player
            GenerateColliderBounding();

            // Log error if portal prefab does not exist.
            if (levelPortalPrefab == null)
            {
                Debug.LogError("The level portal prefab is not assigned!");
                return;
            }

            // Place the 2 level portals at the start and end of the level which will bring us to other levels / areas 
            PlaceLevelPortals(levelPortalPrefab);
            // Spawn player at start portal
            if (Player.Transform != null) // Transport player to start of level.
            {
                Player.Transform.position = startPortal.transform.position;
            }
        }

        void PlaceLevelPortals(GameObject prefab)
        {
            // Find the container to place the level portal game objects in
            Transform container = transform.FindOrCreateChild("LevelPortalsCtn", emptyContent: true).transform;
            // X Offset from the ends of the level 
            float placementOffset = 5;
            // Calculate y offset to place the level portal based on the portal sprite size
            Vector2 yOffset =
                    Vector2.up * (groundLevel + transform.position.y +
                                  prefab.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2);
            // Place the start portal at the start of the level
            startPortal = Instantiate(prefab, LocalLevelLeft + Vector2.right * placementOffset + yOffset, Quaternion.identity, container).GetComponent<PortalInteraction>();
            startPortal.IsStartPortal = transform;
            // Place the end portal at the end of the level
            endPortal = Instantiate(prefab, LocalLevelRight + Vector2.left * placementOffset + yOffset, Quaternion.identity, container).GetComponent<PortalInteraction>();
        }

        /// <summary>
        /// Generates the bounding for the camera confiner (which uses a polygon collider 2D as bounding).
        /// Also generates the edge colliders to block player.
        /// </summary>
        void GenerateColliderBounding()
        {
            // Polygon collider 2d is the camera confiner bounding. 
            PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
            // Edge collider is used to block the player so the player doesnt fall out of the level and stuff
            EdgeCollider2D edge = GetComponent<EdgeCollider2D>();
            // If cannot find poly collider, add it
            if (poly is null) poly = gameObject.AddComponent<PolygonCollider2D>();
            // If cannot find poly collider, add it
            if (edge is null) edge = gameObject.AddComponent<EdgeCollider2D>();

            // Enable edge but disable poly collider (as it is only used for confiner bounding)
            poly.enabled = false;
            edge.enabled = true;
            // Configure the collider with the  correct points
            poly.points = new[] {
                    new Vector2(LocalLevelLeft.x, levelHeight),
                    new Vector2(LocalLevelLeft.x + levelSize, levelHeight),
                    new Vector2(LocalLevelLeft.x + levelSize, 0),
                    new Vector2(LocalLevelLeft.x, 0),
            };
            // Configure the collider with the correct points
            edge.points = new[] {
                    new Vector2(LocalLevelLeft.x, levelHeight),
                    new Vector2(LocalLevelLeft.x + levelSize, levelHeight),
                    new Vector2(LocalLevelLeft.x + levelSize, groundLevel), // y param is ground level so that the player falls to ground level . _ .
                    new Vector2(LocalLevelLeft.x, groundLevel), 
                    new Vector2(LocalLevelLeft.x, levelHeight) // additional point because edge collider doesn't auto loop back
            };
            // Update the camera confiner 
            StartCoroutine(UpdateConfinerCoroutine());
        }

        
        IEnumerator UpdateConfinerCoroutine()
        {
            // Wait a while for things to update before updating the camera confine (by invalidating its cache)
            // Have to wait for it to work.
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