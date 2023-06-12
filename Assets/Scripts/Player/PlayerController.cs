using System.Collections.Generic;
using Entities;
using JetBrains.Annotations;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [CanBeNull] public GameObject PrimaryWeapon;
        [CanBeNull] public GameObject SecondaryWeapon;

        public Transform PrimaryWeaponContainer;
        public Transform SecondaryWeaponContainer;
        // Start is called before the first frame update
        void Start() { }
        
        private void SwitchWeapon(GameObject weaponPrefab, [CanBeNull] ref GameObject weapon,Transform weaponContainer)
        {
            if (weapon)
            {
                Destroy(weapon);
            }
            weapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.identity, weaponContainer);
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
            
        }
    }
}