using UnityEngine;

namespace Core.Utils
{
    /// <summary>
    /// Utility class for creating empty objects to contain other things.
    /// </summary>
    public static class ContainerObjectsUtils
    {
        
        /// <summary>
        /// Tries to find a child game object with the same name, else creates it.
        /// </summary>
        /// <param name="parent">The parent transform</param>
        /// <param name="name">Name of the game object</param>
        /// <param name="worldPosition">Position to assign to the found/created game object</param>
        /// <param name="emptyContent">Whether to destroy all children of the found game object</param>
        /// <returns></returns>
        public static GameObject FindOrCreateChild(this Transform parent, string name, Vector3? worldPosition=null, bool emptyContent = false)
        {
            // Try to find the game object
            var obj = parent.Find(name)?.gameObject;
            // If not found, create it
            if (obj is null) obj = new GameObject(name);
            // Set the parent
            obj.transform.SetParent(parent);
            // If empty content, destroy all children
            if (emptyContent)
            {
                obj.DestroyChildren();
            }
            
            // If a world position is specified, set it
            if (worldPosition != null)
            {
                obj.transform.position = worldPosition.Value;
            }
            // Return the game object
            return obj;
        }


    }
}