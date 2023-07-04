using UnityEngine;

namespace Item
{
    public class WeaponItem : IItemInstance
    {
        public WeaponItemType weaponType;
        public Sprite sprite => weaponType.itemSprite;
        public GameObject prefab => weaponType.itemPrefab;
        public WeaponItem(WeaponItemType weaponType)
        {
            this.weaponType = weaponType;
        }
        
        public string GetDisplayName()
        {
            return weaponType.item_name;
        }

        public string GetDisplayDescription()
        {
            return weaponType.item_description;
        }

        /// <summary>
        /// Calculates & returns the total attack damage of the weapon
        /// </summary>
        /// <returns></returns>
        public float CalculateAttackDamage()
        {
            return weaponType.base_attack; //todo later on: add in a other factors later such as weapon modifiers
        }
    }
}