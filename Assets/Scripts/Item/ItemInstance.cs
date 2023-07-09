using UnityEngine;

namespace Item
{
    public class ItemInstance : IItemInstance
    {
        public ItemScriptableObject item;
        public Sprite sprite => item.itemSprite;
        public GameObject prefab => item.itemPrefab;
        // todo implement stats modifiers.
        public ItemInstance(ItemScriptableObject item)
        {
            this.item = item;
        }

        public string GetDisplayName()
        {
            return item.item_name;
        }

        public string GetDisplayDescription()
        {
            return item.item_description;
        }

        public float Attack => item.base_attack;
        public float Speed => item.base_speed;
        public float Defense => item.base_defense;
        public float Health => item.base_health;
        public float Stamina => item.base_stamina;
        public float Mana => item.base_mana;
    }
}