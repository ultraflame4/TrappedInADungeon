using System;
using Item;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Weapon;

namespace Player
{
    public class WeaponManager : MonoBehaviour
    {
        public readonly int PrimaryWeaponIndex = 0;
        public readonly int SecondaryWeaponIndex = 1;
        private ItemInstance[] equippedWeapons = new ItemInstance[2];
        private GameObject[] weaponObjects = new GameObject[2];
        public PlayerInventory playerInventory;
        [FormerlySerializedAs("offset")] public float WeaponOffset = 0.5f;
        public float weaponTravelSpeed = 0.05f;

        private void Start()
        {
            for (var i = 0; i < playerInventory.itemSlots.Length; i++)
            {
                InventorySlot slot = playerInventory.itemSlots[i];
                if (slot.isWeaponSlot)
                {
                    int slotIndex = i; // make local scope else rider(ide) will complain.
                    slot.onItemChanged += (ItemInstance item) => EquipWeapon(item, slotIndex);
                }
            }
        }


        public void EquipWeapon(ItemInstance weaponInstance, int slotIndex)
        {
            if (weaponInstance is null) // if null empty the slot
            {
                equippedWeapons[slotIndex] = null;
                if (weaponObjects[slotIndex] != null)
                {
                    Destroy(weaponObjects[slotIndex]);
                }

                weaponObjects[slotIndex] = null;
                return;
            }

            if (weaponInstance.itemType is not WeaponItem weapon)
            {
                Debug.LogError($"Error: Tried to equip non-weapon item instance in weapon slot {slotIndex}. itemType: {weaponInstance.itemType}. This probably should not be happening!");
                return;
            }

            equippedWeapons[slotIndex] = weaponInstance;
            if (weaponObjects[slotIndex] != null)
            {
                Destroy(weaponObjects[slotIndex]);
            }

            GameObject obj = Instantiate(weapon.weaponPrefab);
            WeaponController controller = obj.GetComponent<WeaponController>();
            controller.player = transform;
            controller.offset = WeaponOffset + 0.3f*slotIndex;
            controller.travelSpeed = weaponTravelSpeed;
            weaponObjects[slotIndex] = obj;
        }

        public void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weaponObjects[PrimaryWeaponIndex]?.GetComponent<WeaponController>()?.Attack();
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                weaponObjects[SecondaryWeaponIndex]?.GetComponent<WeaponController>()?.Attack();
            }
            if (Input.GetButtonUp("Fire1"))
            {
                weaponObjects[PrimaryWeaponIndex]?.GetComponent<WeaponController>()?.AttackRelease();
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                weaponObjects[SecondaryWeaponIndex]?.GetComponent<WeaponController>()?.AttackRelease();
            }
        }
    }
}