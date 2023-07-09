using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public static class Extensions
    {
        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
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
        public static void SetSprite(this Image image, Sprite sprite=null)
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
    }
}