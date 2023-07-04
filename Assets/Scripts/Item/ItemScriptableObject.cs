using UnityEngine;

namespace Item
{
    public abstract class ItemScriptableObject : ScriptableObject
    {
        public string item_name;
        [Multiline(5)]
        public string item_description;
        public Sprite itemSprite;
        [Tooltip("The prefab to spawn when the item is equipped in the hotbar")]
        public GameObject itemPrefab;
    }
}