using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using Item;
using UI;
using Unity.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private List<ItemInstance> items = new List<ItemInstance>();
        public GameObject inventorySlotsParent;
        [ReadOnly]
        public InventorySlot[] weaponSlots;
        [ReadOnly]
        public InventorySlot[] itemSlots;
        /// <summary>
        /// Just a debug weapon to give to the user (when GiveDebugWeapon is called)
        /// </summary>
        public WeaponItem debugWeapon;

        /// <summary>
        /// This event is called whenever an item is added or removed from the inventory
        /// </summary>
        public event Action inventoryUpdate;


        private void Start()
        {
            // Very overkill way of getting and sorting all the inventory slots using linq extensions ------
            // get all the inventory slots
            var slots = inventorySlotsParent.GetComponentsInChildren<InventorySlot>();
            // group them by whether they are weapon slots or not.
            var groupBy = slots.GroupBy(a => a.isWeaponSlot, b => b, (a, b) => new { isWeaponSlot = a, slots = b.ToArray() }).ToList();
            // find the group that is weapon slots and assign the slots to the weaponSlots variable
            weaponSlots = groupBy.First(x => x.isWeaponSlot).slots;
            // find the group that is item slots and assign the slots to the itemSlots variable
            itemSlots = groupBy.First(x => !x.isWeaponSlot).slots;
            // -----------------------------------------------------------------------
            
        }

        /// <summary>
        /// Adds an item instance to the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public void AddItem<T>(T item) where T : ItemInstance
        {
            items.Add(item);
            inventoryUpdate?.Invoke();
        }

        /// <summary>
        /// Removes the specific item instance
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public void RemoveItem<T>(T item) where T : ItemInstance
        {
            items.Remove(item);
            inventoryUpdate?.Invoke();
        }

        /// <summary>
        /// Removes an item instance by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The item instance removed from the inventory</returns>
        public ItemInstance RemoveAt(int index)
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
        public T[] GetAllItemOfType<T>() where T : ItemInstance
        {
            return items.Where(x => x is T).Cast<T>().ToArray();
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
            AddItem(new WeaponItemInstance(debugWeapon));
        }
    }
}