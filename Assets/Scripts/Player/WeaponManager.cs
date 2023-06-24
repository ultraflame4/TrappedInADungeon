using System;
using Item;
using UI;
using UnityEngine;

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
                    slot.onItemChanged += (ItemInstance item)=>EquipWeapon(item, slotIndex);
                }
            }
        }


        public void EquipWeapon(ItemInstance weapon, int slotIndex)
        {
            equippedWeapons[slotIndex] = weapon;
            if (weaponObjects[slotIndex] != null)
            {
                Destroy(weaponObjects[slotIndex]);
            }
            // weaponObjects[slotIndex] = Instantiate(weapon.itemType.);
        }
    }
}