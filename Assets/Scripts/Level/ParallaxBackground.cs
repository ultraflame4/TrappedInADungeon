using System;
using EasyButtons;
using UnityEngine;
using Utils;

namespace Level
{
    public class ParallaxBackground : MonoBehaviour
    {
        public Sprite[] layers;
        public Color colorFrom = Color.white;
        public Color colorTo = Color.black;
        private GameObject[] layerObjects;

        // Start is called before the first frame update
        [Button("Generate Layers")]
        void Start()
        {
            transform.DestroyChildren();
            layerObjects = new GameObject[layers.Length];
            for (var i = 0; i < layers.Length; i++)
            {
                var sprite = layers[i];
                GameObject gameObj = new($"Layer {i}");
                layerObjects[i] = gameObj;
                gameObj.transform.parent = transform;
                gameObj.transform.localPosition = Vector3.zero;
                var renderer = gameObj.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                renderer.color = Color.Lerp(colorFrom, colorTo, (float)i / (layers.Length - 1));
            }
        }


        // Update is called once per frame
        void Update() { }
    }
}