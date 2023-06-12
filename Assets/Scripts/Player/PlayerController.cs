using System;
using System.Collections.Generic;
using Entities;
using JetBrains.Annotations;
using UnityEngine;
using Weapon;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [CanBeNull] public WeaponController PrimaryWeapon;
        [CanBeNull] public WeaponController SecondaryWeapon;

        public Transform PrimaryWeaponContainer;
        public Transform SecondaryWeaponContainer;
        // Start is called before the first frame update
        void Start() { }
        
        private void SwitchWeapon(GameObject weaponPrefab, [CanBeNull] ref WeaponController weapon,Transform weaponContainer)
        {
            if (weapon)
            {
                Destroy(weapon);
            }
            var gameObject = Instantiate(weaponPrefab, Vector3.zero, Quaternion.identity, weaponContainer);
            var weapon_ = gameObject.GetComponent<WeaponController>();
            if (weapon_ is null)
            {
                throw new NullReferenceException($"Error, equipped weapon prefab does not have a weapon controller! {weaponPrefab}");
            }
            weapon = weapon_;
        }
        public void SwitchPrimaryWeapon(GameObject newWeaponPrefab)
        {
            SwitchWeapon(newWeaponPrefab, ref PrimaryWeapon, PrimaryWeaponContainer);
        }
        public void SwitchSecondaryWeapon(GameObject newWeaponPrefab)
        {
            SwitchWeapon(newWeaponPrefab, ref SecondaryWeapon,SecondaryWeaponContainer);
        }

        // Update is called once per frame
        void Update()
        {
            // todo change letter to support holding down stuff
            if (Input.GetButtonDown("Fire1"))
            {
                PrimaryWeapon?.Attack();
            }

            if (Input.GetButtonDown("Fire3"))
            {
                SecondaryWeapon?.Attack();
            }
        }
    }
}