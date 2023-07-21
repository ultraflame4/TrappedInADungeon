using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using Item;
using UnityEngine;

namespace UI.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        private List<ItemInstance> items = new ();
        public GameObject inventorySlotsParent;
        [Tooltip("The inventory slots in the ui. Automatically found by the script")]
        public InventorySlot[] itemSlots;
        [Tooltip("Just a debug weapon to give to the user (when GiveDebugWeapon is called)")]
        public ItemScriptableObject debugWeapon;
        [Tooltip("Skill to give to the player (when GiveDebugSkill is called)")]
        public ItemScriptableObject debugSkill;
        [Tooltip("This event is invoked whenever an item is added or removed from the inventory")]
        public event Action InventoryUpdate;
        [Tooltip("All items in the inventory")]
        public List<ItemInstance> AllItems => items.ToList();

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
        public void AddItem<T>(T item) where T : ItemInstance
        {
            items.Add(item);
            InventoryUpdate?.Invoke();
        }

        /// <summary>
        /// Removes the specific item instance
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public void RemoveItem<T>(T item) where T : ItemInstance
        {
            items.Remove(item);
            InventoryUpdate?.Invoke();
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
            InventoryUpdate?.Invoke();
            return obj;
        }

        /// <summary>
        /// Returns all item instances in the inventory that matches type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ItemInstance[] GetAllItemOfType(ItemType itemType)
        {
            return items.Where(x => x.item.itemType==itemType).ToArray();
        }

        /// <summary>
        /// Clears the player inventory
        /// Mainly for debugging purposes
        /// </summary>
        [Button]
        public void Clear()
        {
            items.Clear();
            InventoryUpdate?.Invoke();
        }

        /// <summary>
        /// Gives the player a debug weapon
        /// Mainly for debugging purposes
        /// </summary>
        [Button]
        public void GiveDebugWeapon()
        {
            AddItem(new ItemInstance(debugWeapon));
        }
        /// <summary>
        /// Gives the player a fireball
        /// Mainly for debugging purposes
        /// </summary>
        [Button]
        public void GiveDebugSkill()
        {
            AddItem(new ItemInstance(debugSkill));
        }
    }
}