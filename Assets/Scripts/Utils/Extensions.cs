using UnityEngine;

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
    }
}