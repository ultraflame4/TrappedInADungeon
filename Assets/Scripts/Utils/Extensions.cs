using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public static class Extensions
    {
        public static void DestroyChildren(this Transform transform)
        {
            // Store the children in a list first as there are some problems with looping through the children in the transform directly
            // which casues some children to not be destroyed
            // probably has something to do with the fact that the collection is modified while looping through it
            List<Transform> children = transform.Cast<Transform>().ToList();
            foreach (var child in children)
            {
                if (!Application.isPlaying)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                    continue;
                }

                GameObject.Destroy(child.gameObject);
            }
        }

        public static void DestroyChildren(this GameObject gameObject)
        {
            gameObject.transform.DestroyChildren();
        }

        /// <summary>
        /// Sets the sprite of an image, and automatically enables/disables the image if the sprite is null or not.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="sprite"></param>
        public static void SetSprite(this Image image, Sprite sprite = null)
        {
            image.sprite = sprite;
            image.enabled = sprite != null;
        }

        public static string JoinString(this IEnumerable<string> strings, string separator = ",")
        {
            return string.Join(separator, strings);
        }

        public static bool IsNegative(this float value)
        {
            return value < 0;
        }

        public static T Add<T>(this List<T> a, T b)
        {
            a.Add(b);
            return b;
        }

        public static Vector2 WorldToCanvasPoint(this Canvas canvas, Vector3 worldPosition, Camera camera = null)
        {
            camera = camera ?? Camera.main;
            Vector2 viewport = camera.WorldToViewportPoint(worldPosition); // get viewport space
            var centered = viewport - Vector2.one * .5f; // convert to center based viewport (because unity viewport's origin is left bottom)
            // multiply by the canvas sizeDelta to get the actual position on the canvas
            Vector2 canvasPoint = Vector2.Scale(centered,canvas.GetComponent<RectTransform>().sizeDelta);
            return canvasPoint;
        }
    }
}