using UnityEngine;

namespace Item
{
    public class WeaponItem : IItemInstance
    {
        public WeaponItemType weaponType;
        public Sprite sprite => weaponType.itemSprite;
        
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

        
    }
}