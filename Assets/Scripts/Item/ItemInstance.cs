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
    }
}