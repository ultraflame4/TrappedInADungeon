using System;
using Item;
using UI.Dragging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class InventorySlot : MonoBehaviour
    {
        public Image itemImage;

        /// <summary>
        /// Set this to true if this inventory slot is meant to hold weapons.
        /// Weapon slots can only hold weapons. non-Weapon slots can hold anything except weapons.
        /// </summary>
        public bool isWeaponSlot = false;

        public event Action<ItemInstance> itemChanged; 
        private ItemInstance itemInstance;

        /// <summary>
        /// Sets the current item instance in this slot.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Returns true if successful, vice versa</returns>
        public bool SetItem(ItemInstance item)
        {
            if (item is WeaponItemInstance)
            {
                if (!isWeaponSlot) return false;
                _SetItem(item);
                return true;
            }
            if (isWeaponSlot) return false;
            _SetItem(item);
            return true;
        }

        private void _SetItem(ItemInstance item)
        {
            itemInstance = item;
            itemImage.SetSprite(itemInstance.itemType.itemSprite);
            itemChanged?.Invoke(itemInstance);
        }
    }
}