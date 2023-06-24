using System;
using Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class InventorySlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        public Image itemImage;
        public SpriteButton spriteButton;
        /// <summary>
        /// The input button name to use for this slot. If this is set, the slot will be activated when the button is pressed.
        /// </summary>
        public string buttonName = "";
        /// <summary>
        /// Set this to true if this inventory slot is meant to hold weapons.
        /// Weapon slots can only hold weapons. non-Weapon slots can hold anything except weapons.
        /// </summary>
        public bool isWeaponSlot = false;
        /// <summary>
        /// This event is fired whenever the item in this slot changes. The value passed is the new item instance or null (if cleared).
        /// </summary>
        public event Action<ItemInstance> onItemChanged;
        /// <summary>
        /// This event is fired whenever the item in this slot is used. The value passed is the current item instance or null (if empty).
        /// </summary>
        public event Action<ItemInstance> onItemUsed;
        
        private ItemInstance itemInstance = null;
        
        
        void Update()
        {
            if (buttonName.Length != 0)
            {
                if (Input.GetButtonDown(buttonName))
                {
                    spriteButton.activeOverride = true;
                    onItemUsed?.Invoke(itemInstance);
                    spriteButton.UpdateImageSprite();
                }
                else if (Input.GetButtonUp(buttonName))
                {
                    spriteButton.activeOverride = false;
                    spriteButton.UpdateImageSprite();
                }
            }
        }

        /// <summary>
        /// Sets the current item instance in this slot.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Returns true if successful, vice versa</returns>
        public bool SetItem(ItemInstance item)
        {
            if (item == null)
            {
                _SetItem(null);
                return true;
            }
            
            if (item.itemType is WeaponItem)
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
            itemImage.SetSprite(itemInstance?.itemType.itemSprite);
            onItemChanged?.Invoke(itemInstance);
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // dont allow dragging if there is no item in this slot
            if (itemInstance == null) return;
            // hide item image
            itemImage.enabled = false;
            CursorController.GetInstance().StartDrag(this, itemInstance.itemType.itemSprite);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // if the item was not dropped in another slot, count as user throwing the item into the void
            if (!CursorController.GetInstance().optionalDropSuccess)
            {
                SetItem(null);
            }
            // show item image if there is an item in this slot
            itemImage.enabled = itemInstance != null;
            CursorController.GetInstance().EndDrag();
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggedData = CursorController.GetInstance().GetDraggedData();
            if (draggedData is ItemInstance item)
            {
                SetItem(item);
            }
            else if (draggedData is InventorySlot inventorySlot)
            {
                item = inventorySlot.itemInstance; // get item from the other slot
                if (SetItem(item)) // if successfully set item
                {
                    inventorySlot.SetItem(null); // clear the item in the other slot
                }
                // Tell the other slot that the item was dropped in another slot (regardless of whether the inventorySlot.SetItem() was successful
                CursorController.GetInstance().optionalDropSuccess = true; 
            }
        }
    }
}