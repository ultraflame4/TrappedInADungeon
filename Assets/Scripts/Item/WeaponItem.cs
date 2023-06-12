using System;
using UnityEngine;
using Weapon;

namespace Item
{
    [CreateAssetMenu(fileName = "weapon_item",menuName = "GameContent/Items/Weapon")]
    public class WeaponItem: ItemScriptableObject
    {
        public GameObject weaponPrefab;
        public float base_attack;

    }
}