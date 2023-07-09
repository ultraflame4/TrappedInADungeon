using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "game_item",menuName = "GameContent/Item")]
    public sealed class ItemScriptableObject : ScriptableObject
    {
        [Tooltip("Name of the item shown in inventory")]
        public string item_name;
        [Tooltip("Description of item shown in inventory"),Multiline(5)]
        public string item_description;
        [Tooltip("Sprite to display in the inventory")]
        public Sprite itemSprite;
        [Tooltip("The prefab to spawn when the item is equipped in the hotbar")]
        public GameObject itemPrefab;
        [Tooltip("The amount of damage this item deals when used.")]
        public float base_attack;
        [Tooltip("The amount of defense this item provides when used.")]
        public float base_defense;
        [Tooltip("The amount of speed this item provides when used.")]
        public float base_speed;
        [Tooltip("The amount of health this item provides when used.")]
        public float base_health;
        [Tooltip("The amount of stamina this item provides when used.")]
        public float base_stamina;
        [Tooltip("The amount of mana this item provides when used.")]
        public float base_mana;
        [Tooltip("The amount of mana this item costs when used.")]
        public float mana_cost;
    }
}