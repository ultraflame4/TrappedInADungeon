using UnityEngine;

namespace Item
{
    public class WeaponItem : IItemInstance
    {
        public ItemScriptableObject weaponItem;
        public Sprite sprite => weaponItem.itemSprite;
        public GameObject prefab => weaponItem.itemPrefab;
        public WeaponItem(ItemScriptableObject weaponItem)
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

        /// <summary>
        /// Calculates & returns the total attack damage of the weapon
        /// </summary>
        /// <returns></returns>
        public float CalculateAttackDamage()
        {
            return weaponItem.base_attack; //todo later on: add in a other factors later such as weapon modifiers
        }
    }
}