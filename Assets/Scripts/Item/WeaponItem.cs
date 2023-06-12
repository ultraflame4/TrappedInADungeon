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

        public WeaponController GetWeaponController()
        {
            var controller = weaponPrefab.GetComponent<WeaponController>();
            if (controller == null)
            {
                throw new NullReferenceException($"Error! Weapon Prefab for this Weapon Item ({item_name}) does not have a weapon controller!");
            }

            return controller;
        }
    }
}