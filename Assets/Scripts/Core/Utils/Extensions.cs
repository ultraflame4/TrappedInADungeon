using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Utils
{
    public static class Extensions
    {
        public static void DestroyChildren(this Transform transform)
        {
            // Store the children in a list first as there are some problems with looping through the children in the transform directly
            // which causes some children to not be destroyed
            // probably has something to do with the fact that the collection is modified while looping through it
            List<Transform> children = transform.Cast<Transform>().ToList();
            // Loop through the children and destroy them
            foreach (var child in children)
            {
                if (!Application.isPlaying) // If we are in the editor, use the immediate version of destroy
                {
                    GameObject.DestroyImmediate(child.gameObject);
                    continue;
                }

                GameObject.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Destroys all children of a gameobject
        /// </summary>
        /// <param name="gameObject"></param>
        public static void DestroyChildren(this GameObject gameObject)
        {
            // Call the transform version of this method
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

        /// <summary>
        /// Joins a bunch of strings together with a separator. Similar to string.Join but operates more like python's "".join("\n").<br/>
        /// Useful for quick debugging and logging.
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string JoinString(this IEnumerable<string> strings, string separator = ",")
        {
            return string.Join(separator, strings);
        }
        /// <summary>
        /// Whether the float value is nefative
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNegative(this float value)
        {
            return value < 0;
        }
        /// <summary>
        /// Converts a world position to a canvas position for use in UI elements
        /// </summary>
        /// <param name="canvas">The canvas</param>
        /// <param name="worldPosition">World position to convert</param>
        /// <param name="camera">Camera to use for conversion. Defaults to Camera.main</param>
        /// <returns></returns>
        public static Vector2 WorldToCanvasPoint(this Canvas canvas, Vector3 worldPosition, Camera camera = null)
        {
            camera = camera ?? Camera.main; // If no camera is specified, use the main camera
            Vector2 viewport = camera.WorldToViewportPoint(worldPosition); // get viewport space
            var centered = viewport - Vector2.one * .5f; // convert to center based viewport (because unity viewport's origin is left bottom)
            // multiply by the canvas sizeDelta to get the actual position on the canvas
            Vector2 canvasPoint = Vector2.Scale(centered,canvas.GetComponent<RectTransform>().sizeDelta);
            return canvasPoint;
        }

        /// <summary>
        /// Rounds a float to a specified number of decimal places
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimal_places"></param>
        /// <returns></returns>
        public static float ToPrecision(this float number, int decimal_places)
        {
            float power = Mathf.Pow(10, decimal_places);
            return Mathf.Round(number * power) / power;
        }
        
        /// <summary>
        /// Shorthand for Path.GetFullPath so you can do something like "abc".FullPath()
        /// </summary>
        /// <param name="pathString"></param>
        /// <returns></returns>
        public static string FullPath(this string pathString)
        {
            return Path.GetFullPath(pathString);
        }
        
        /// <summary>
        /// Cleans a string of all non-ascii characters
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Clean(this string s)
        {
            // List of valid characters
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_ ";
            // Iterate through each character in the string and only keep the valid ones
            return s.Where(x => validCharacters.Contains(x)).Aggregate("", (current, x) => current + x);
        }
    }
}