using Item;
using UI.Inventory;
using UnityEngine;
using UnityEngine.Serialization;
using Weapon;

namespace Player
{
    public class WeaponManager : MonoBehaviour
    {
        public readonly int PrimaryWeaponIndex = 0;
        public readonly int SecondaryWeaponIndex = 1;
        private WeaponItem[] equippedWeapons = new WeaponItem[2];
        private GameObject[] weaponObjects = new GameObject[2];
        public PlayerInventory playerInventory;
        [FormerlySerializedAs("offset")] public float WeaponOffset = 0.5f;
        public float weaponTravelSpeed = 0.4f;
        public float weaponAttackTravelSpeed = 0.8f;

        private void Start()
        {
            for (var i = 0; i < playerInventory.itemSlots.Length; i++)
            {
                InventorySlot slot = playerInventory.itemSlots[i];
                if (slot.isWeaponSlot)
                {
                    int slotIndex = i; // make local scope else rider(ide) will complain.
                    slot.onItemChanged += (item) => EquipWeapon(item as WeaponItem, slotIndex);
                }
            }
        }

        public void EquipWeapon(WeaponItem weaponItem, int slotIndex)
        {
            if (weaponItem is null) // if null empty the slot
            {
                equippedWeapons[slotIndex] = null;
                if (weaponObjects[slotIndex] != null)
                {
                    Destroy(weaponObjects[slotIndex]);
                }

                weaponObjects[slotIndex] = null;
                return;
            }


            equippedWeapons[slotIndex] = weaponItem;
            if (weaponObjects[slotIndex] != null)
            {
                Destroy(weaponObjects[slotIndex]);
            }

            GameObject obj = Instantiate(weaponItem.weaponType.weaponPrefab);
            WeaponController controller = obj.GetComponent<WeaponController>();
            controller.player = transform;
            controller.follow_offset = WeaponOffset + 0.3f * slotIndex;
            controller.travelSpeed = weaponTravelSpeed;
            controller.attackingTravelSpeed = weaponAttackTravelSpeed;
            controller.weaponItem = weaponItem;
            weaponObjects[slotIndex] = obj;
        }

        public void Update()
        {
            if (GameManager.Controls.Hotbar.Primary.WasPerformedThisFrame())
            {
                weaponObjects[PrimaryWeaponIndex]?.GetComponent<WeaponController>()?.Attack();
            }

            if (GameManager.Controls.Hotbar.Secondary.WasPerformedThisFrame())
            {
                weaponObjects[SecondaryWeaponIndex]?.GetComponent<WeaponController>()?.Attack();
            }

            if (GameManager.Controls.Hotbar.Primary.WasReleasedThisFrame())
            {
                weaponObjects[PrimaryWeaponIndex]?.GetComponent<WeaponController>()?.AttackRelease();
            }

            if (GameManager.Controls.Hotbar.Secondary.WasReleasedThisFrame())
            {
                weaponObjects[SecondaryWeaponIndex]?.GetComponent<WeaponController>()?.AttackRelease();
            }
        }
    }
}