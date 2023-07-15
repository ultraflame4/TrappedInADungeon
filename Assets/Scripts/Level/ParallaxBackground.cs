using EasyButtons;
using UnityEngine;
using Utils;

namespace Level
{
    public class ParallaxBackground : MonoBehaviour
    {
        public Ground ground;
        public Sprite[] layers;
        public Color colorFrom = Color.white;
        public Color colorTo = Color.black;
        public float parallaxFactor;
        public int sections = 3;
        public float yOffset;
        private GameObject[] layerObjects;
        private Transform camera;

        private Vector3 startPos;
        public float TotalWidth => layers[0].bounds.size.x * sections;

        // Start is called before the first frame update

        void Start()
        {
            startPos = transform.position;
            camera = GameObject.FindWithTag("MainCamera").transform;
            GenerateLayers();
        }

        [Button("Generate Layers")]
        void GenerateLayers()
        {
            transform.DestroyChildren();
            ground.UpdateWidth(TotalWidth,sections);
            layerObjects = new GameObject[layers.Length];
            for (var i = 0; i < layers.Length; i++)
            {
                
                GameObject layerObj = new($"Layer Container {i}");
                layerObjects[i] = layerObj;
                layerObj.transform.parent = transform;
                layerObj.transform.localPosition = new Vector3(0,yOffset,-i);
                for (int j = 0; j < sections; j++)
                {
                    GenerateSection(layerObj.transform,i,j);
                }
                
            }
        }

        void GenerateSection(Transform parent, int layerIndex, int sectionIndex)
        {
            var sprite = layers[layerIndex];
            GameObject gameObj = new($"Layer {layerIndex}");
            gameObj.transform.parent = parent;
            // -0.01f to prevent tiny gap between sections
            gameObj.transform.localPosition = Vector3.right * (sprite.bounds.size.x-0.01f) * sectionIndex;
            var renderer = gameObj.AddComponent<SpriteRenderer>();
            renderer.sortingLayerID = SortingLayer.NameToID("Background");
            renderer.sprite = sprite;
            renderer.color = Color.Lerp(colorFrom, colorTo, (float)layerIndex / (layers.Length - 1));
        }

        // Update is called once per frame
        void Update()
        {
            if (camera == null) return;
            float travelX = (camera.position - startPos).x;
            for (var i = 0; i < layerObjects.Length; i++)
            {
                var layer = layerObjects[i];
                float layerPercent = (float)i / (layerObjects.Length - 1);
                Vector3 layerPos = layer.transform.position;
                layerPos.x = startPos.x + travelX * layerPercent * parallaxFactor;
                layer.transform.position = layerPos;
            }
        }
    }
}