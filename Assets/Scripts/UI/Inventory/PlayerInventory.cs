﻿using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using Item;
using UnityEngine;

namespace UI.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        private List<IItemInstance> items = new ();
        public GameObject inventorySlotsParent;
        
        public InventorySlot[] itemSlots;
        /// <summary>
        /// Just a debug weapon to give to the user (when GiveDebugWeapon is called)
        /// </summary>
        public ItemScriptableObject debugWeapon;

        /// <summary>
        /// This event is called whenever an item is added or removed from the inventory
        /// </summary>
        public event Action inventoryUpdate;


        private void Awake()
        {
            itemSlots = inventorySlotsParent.GetComponentsInChildren<InventorySlot>(); // automatically finds all item slots
            for (var i = 0; i < itemSlots.Length; i++)
            {
                itemSlots[i].slotIndex = i; // set the slot index fpr each slot
            }
        }

        /// <summary>
        /// Adds an item instance to the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public void AddItem<T>(T item) where T : IItemInstance
        {
            items.Add(item);
            inventoryUpdate?.Invoke();
        }

        /// <summary>
        /// Removes the specific item instance
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public void RemoveItem<T>(T item) where T : IItemInstance
        {
            items.Remove(item);
            inventoryUpdate?.Invoke();
        }

        /// <summary>
        /// Removes an item instance by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The item instance removed from the inventory</returns>
        public IItemInstance RemoveAt(int index)
        {
            var obj = items[index];
            items.RemoveAt(index);
            inventoryUpdate?.Invoke();
            return obj;
        }

        /// <summary>
        /// Returns all item instances in the inventory that matches type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IItemInstance[] GetAllItemOfType<T>() where T : class, IItemInstance
        {
            return items.Where(x => x is T).ToArray();
        }

        /// <summary>
        /// Clears the player inventory
        /// Mainly for debugging purposes
        /// </summary>
        [Button]
        public void Clear()
        {
            items.Clear();
            inventoryUpdate?.Invoke();
        }

        /// <summary>
        /// Gives the player a debug weapon
        /// Mainly for debugging purposes
        /// </summary>
        [Button]
        public void GiveDebugWeapon()
        {
            AddItem(new WeaponItem(debugWeapon));
        }
    }
}