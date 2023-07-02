using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "weapon_item",menuName = "GameContent/Items/Weapon")]
    public class WeaponItemType: ItemScriptableObject
    {
        public GameObject weaponPrefab;
        public float base_attack;
        public AnimatorOverrideController animatorOverrideController;
    }
}