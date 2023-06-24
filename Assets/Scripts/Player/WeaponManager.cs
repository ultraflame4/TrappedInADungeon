using System;
using Item;
using UI;
using UnityEngine;
using Weapon;

namespace Player
{
    public class WeaponManager : MonoBehaviour
    {
        public readonly int PrimaryWeaponIndex = 0;
        public readonly int SecondaryWeaponIndex = 0;
        private ItemInstance[] equippedWeapons = new ItemInstance[2];
        private GameObject[] weaponObjects = new GameObject[2];
        public PlayerInventory playerInventory;

        private void Start()
        {
            for (var i = 0; i < playerInventory.itemSlots.Length; i++)
            {
                InventorySlot slot = playerInventory.itemSlots[i];
                if (slot.isWeaponSlot)
                {
                    int slotIndex = i; // make local scope else rider(ide) will complain.
                    slot.onItemChanged += (ItemInstance item)=>EquipWeapon(item , slotIndex);
                }
            }
        }


        public void EquipWeapon(ItemInstance weaponInstance, int slotIndex)
        {
            WeaponItem weapon = weaponInstance.itemType as WeaponItem;
            if (weapon is null)
            {
                Debug.LogError($"Error: Tried to equip non-weapon item instance in weapon slot {slotIndex}. weaponInstance: {weaponInstance}. This probably should not be happening!");
                return;
            }
            
            equippedWeapons[slotIndex] = weaponInstance;
            if (weaponObjects[slotIndex] != null)
            {
                Destroy(weaponObjects[slotIndex]);
            }
            GameObject obj = Instantiate(weapon.weaponPrefab);
            obj.AddComponent<WeaponController>();
            weaponObjects[slotIndex] = obj;
        }
    }
}