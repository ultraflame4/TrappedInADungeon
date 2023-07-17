using System;
using EasyButtons;
using UnityEngine;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        public Ground ground;
        private ParallaxBackground background;

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
            if (background is null) throw new NullReferenceException("Could not find ParallaxBackground component in children of LevelManager");
            if (ground is null) throw new NullReferenceException("Could not find Ground component in children of LevelManager");
        }
        
        [Button]
        public void GenerateLevel()
        {
            GetRequiredComponents();
            background.sections = Mathf.CeilToInt(levelSize / background.SectionWidth);
            background.xOffset = -(background.TotalWidth-background.SectionWidth) / 2;
            background.GenerateLayers();
            ground.UpdateWidth(levelSize);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(levelSize, levelHeight, 1));
            
        }
    }
}