using UnityEngine;

namespace Item
{
    public class WeaponItem : ItemInstance
    {
        public WeaponItem(ItemScriptableObject weaponItem) : base(weaponItem) { }
        /// <summary>
        /// Calculates & returns the total attack damage of the weapon
        /// </summary>
        /// <returns></returns>
        public float CalculateAttackDamage()
        {
            return item.base_attack; //todo later on: add in a other factors later such as weapon modifiers
        }


    }
}