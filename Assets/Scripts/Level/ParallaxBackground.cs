using EasyButtons;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Level
{
    public class ParallaxBackground : MonoBehaviour
    {
        [Tooltip("Layers of the background. The first layer is the furthest away.")]
        public Sprite[] layers;
        [FormerlySerializedAs("ground"),Tooltip("The sprite to use for the ground layer.")]
        public Sprite groundSprite;
        [Tooltip("Y offset of the ground layer.")]
        public float groundYOffset;
        [Tooltip("The start color to tint to layers.")]
        public Color colorFrom = Color.white;
        [Tooltip("The end color to tint to layers.")]
        public Color colorTo = Color.black;
        [Tooltip("How much the layers move relative to the camera.")]
        public float parallaxFactor;
        [Tooltip("How many sections to generate. The more sections, wider the background.")]
        public int sections = 3;
        [Tooltip("How much to offset the background in the y axis.")]
        public float yOffset;
        [Tooltip("How much to offset the background in the x axis.")]
        public float xOffset=0;
        /// <summary>
        /// The game objects that contain the layers.
        /// </summary>
        private GameObject[] layerObjects;
        /// <summary>
        /// The camera transform.
        /// </summary>
        private Transform cam;
        /// <summary>
        /// The initial position of the background.
        /// </summary>
        private Vector3 startPos;
        /// <summary>
        /// Width of a single section.
        /// </summary>
        public float SectionWidth => layers[0].bounds.size.x;
        /// <summary>
        /// Width of the entire background. Sections * SectionWidth
        /// </summary>
        public float TotalWidth => SectionWidth * sections;

        // Start is called before the first frame update

        void Start()
        {
            startPos = transform.position;
            cam = GameObject.FindWithTag("MainCamera").transform;
            // GenerateLayers();
        }

        [Button("Generate Layers")]
        public void GenerateLayers()
        {
            transform.DestroyChildren(); // Destroy all children
            layerObjects = new GameObject[layers.Length]; // Create a new array to store the layer objects
            for (var i = 0; i < layers.Length; i++)
            {
                GameObject layerObj = new($"Layer Container {i}"); // Create a new game object to store the layer sections
                layerObjects[i] = layerObj;
                layerObj.transform.parent = transform; // Set the parent to this object
                layerObj.transform.localPosition = new Vector3(xOffset,yOffset,-i); // Set the position of the layer
                for (int j = 0; j < sections; j++) // Generate the sections
                {
                    GenerateSection(layerObj.transform,layers[i],$"Layer {i}",(float)i / (layers.Length),j); 
                }
                
            }

            GameObject groundLayerObj = new($"Ground Layer Container"); // Create a new game object to store the layer sections
            groundLayerObj.transform.parent = transform; // Set the parent to this object
            groundLayerObj.transform.localPosition = new Vector3(xOffset,groundYOffset,-layers.Length); // Set the position of the layer
            for (int i = 0; i < sections; i++) // Generate the sections
            {
                GenerateSection(groundLayerObj.transform,groundSprite,$"Ground Section {i}",1,i); 
            }
     
        }

        void GenerateSection(Transform parent, Sprite sprite,string sectionName, float colorMix, int sectionIndex)
        {
            GameObject gameObj = new(sectionName); // Create a new game object to render the sprite
            gameObj.transform.parent = parent; // Set the parent to the layer object
            // -0.01f to prevent tiny gap between sections
            gameObj.transform.localPosition = Vector3.right * (sprite.bounds.size.x-0.01f) * sectionIndex; // Set the position of this section
            var spriteRenderer = gameObj.AddComponent<SpriteRenderer>(); // Add a sprite renderer to render the sprite
            // Sprite renderer settings
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Background");
            spriteRenderer.sprite = sprite;
            // Tint the sprite
            spriteRenderer.color = Color.Lerp(colorFrom, colorTo, colorMix);
        }

        // Update is called once per frame
        void Update()
        {
            if (cam == null) return;
            float travelX = (cam.position - startPos).x;
            for (var i = 0; i < layerObjects.Length; i++)
            {
                var layer = layerObjects[i];
                float layerPercent = (float)i / (layerObjects.Length - 1);
                Vector3 layerPos = layer.transform.position;
                layerPos.x = xOffset + startPos.x + travelX * layerPercent * parallaxFactor;
                layer.transform.position = layerPos;
            }
        }
    }
}