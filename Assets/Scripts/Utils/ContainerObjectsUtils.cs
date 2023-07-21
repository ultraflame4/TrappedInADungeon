using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Utility class for creating empty objects to contain other things.
    /// </summary>
    public static class ContainerObjectsUtils
    {

        public static GameObject CreateEmpty(this Transform parent, string name, Vector3? worldPosition=null)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent);
            if (worldPosition != null)
            {
                obj.transform.position = worldPosition.Value;
            }

            return obj;
        }
        /// <summary>
        /// Tries to find a child game object with the same name, else creates it.
        /// </summary>
        /// <param name="parent">The parent transform</param>
        /// <param name="name">Name of the gameobject</param>
        /// <param name="worldPosition">Position to assign to the found/created game object</param>
        /// <param name="emptyContent">Whether to destroy all children of the found game object</param>
        /// <returns></returns>
        public static GameObject FindOrCreateChild(this Transform parent, string name, Vector3? worldPosition=null, bool emptyContent = false)
        {
            var obj = parent.Find(name)?.gameObject;
            if (obj is null) obj = new GameObject(name);
            obj.transform.SetParent(parent);
            if (emptyContent)
            {
                obj.DestroyChildren();
            }
            if (worldPosition != null)
            {
                obj.transform.position = worldPosition.Value;
            }

            return obj;
        }
    }
}