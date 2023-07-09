using UnityEngine;

namespace Item
{
    public class ItemInstance : IItemInstance
    {
        public ItemScriptableObject weaponItem;
        public Sprite sprite => weaponItem.itemSprite;
        public GameObject prefab => weaponItem.itemPrefab;
        
        public ItemInstance(ItemScriptableObject weaponItem)
        {
            this.weaponItem = weaponItem;
        }

        public string GetDisplayName()
        {
            return weaponItem.item_name;
        }

        public string GetDisplayDescription()
        {
            return weaponItem.item_description;
        }

        public float Attack => weaponItem.base_attack;
        public float Speed => weaponItem.base_speed;
        public float Defense => weaponItem.base_defense;
        public float Health => weaponItem.base_health;
        public float Stamina => weaponItem.base_stamina;
        public float Mana => weaponItem.base_mana;
    }
}