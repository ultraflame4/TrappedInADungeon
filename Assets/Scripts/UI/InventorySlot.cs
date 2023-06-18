﻿using System;
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

        /// <summary>
        /// Set this to true if this inventory slot is meant to hold weapons.
        /// Weapon slots can only hold weapons. non-Weapon slots can hold anything except weapons.
        /// </summary>
        public bool isWeaponSlot = false;

        public event Action<ItemInstance> itemChanged;
        private ItemInstance itemInstance = null;

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
            itemImage.SetSprite(itemInstance?.itemType.itemSprite);
            itemChanged?.Invoke(itemInstance);
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // hide item image
            itemImage.enabled = false;
            CursorController.GetInstance().StartDrag(this, itemInstance.itemType.itemSprite);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
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
            }
        }
    }
}