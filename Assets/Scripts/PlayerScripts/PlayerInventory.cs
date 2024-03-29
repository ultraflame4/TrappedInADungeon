﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Item;
using Core.Save;
using Core.UI;
using EasyButtons;
using Newtonsoft.Json;
using UI.Inventory;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerInventory : MonoBehaviour, ISaveHandler
    {
        [field: SerializeField, Tooltip("List of items to spawn with if player has no items.")]
        public ItemInstance[] defaultItems { get; private set; }

        [SerializeField, JsonProperty]
        private List<ItemInstance> items = new();

        [Tooltip("The inventory slots in the ui. Automatically found by the script")]
        public InventorySlot[] itemSlots;

        [Tooltip("Just a debug weapon to give to the user (when GiveDebugWeapon is called)")]
        public ItemScriptableObject debugWeapon;

        [Tooltip("Skill to give to the player (when GiveDebugSkill is called)")]
        public ItemScriptableObject debugSkill;

        [Tooltip("This event is invoked whenever an item is added or removed from the inventory")]
        public event Action InventoryUpdate;

        /// <summary>
        /// Returns a shallow list copy of all items in the inventory. Safe to loop over.
        /// </summary>
        [Tooltip("All items in the inventory")]
        public List<ItemInstance> AllItems => items.ToList();

        private void Awake()
        {
            // Register this object to be saved & loaded
            GameSaveManager.AddSaveHandler("player.inventory", this);
            // Set player inventory default items
            ResetInventory();
        }

        void Start()
        {
            // Send out event at end of this frame to force child listeners to update
            StartCoroutine(LateStart());
        }

        IEnumerator LateStart()
        {
            // Wait till end of frame to wait for listener registration
            yield return new WaitForEndOfFrame();
            InventoryUpdate?.Invoke();
        }

        /// <summary>
        /// Reset inventory to the default items
        /// </summary>
        public void ResetInventory()
        {
            items = defaultItems.ToList();
        }

        /// <summary>
        /// Adds an item instance to the inventory. Note that this will also automatically try to combine the itemInstance to a similar itemInstance.
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public void AddItem<T>(T item) where T : ItemInstance
        {
            // push helpful notifications :D
            NotificationManager.Instance.PushNotification($"Picked up <color=#41f0d0>{item.item.itemType} - {item.item.item_name}!</color>");
            // Search through all items
            foreach (ItemInstance itemInstance in AllItems)
            {
                // If current item combines successfully, invoke event & return
                if (itemInstance.Combine(item))
                {
                    InventoryUpdate?.Invoke();
                    return;
                }
            }

            items.Add(item);
            // inventory has updated, invoke event.
            InventoryUpdate?.Invoke();
        }

        /// <summary>
        /// Removes the specific item instance
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(ItemInstance item)
        {
            items.Remove(item);
            InventoryUpdate?.Invoke(); // inventory has updated, invoke event.
        }
        

        /// <summary>
        /// Adjusts number of items in the specified itemInstance & invokes inventory update event
        /// </summary>
        /// <param name="itemInstance"></param>
        /// <param name="newCount"></param>
        public void AdjustItemCount(ItemInstance itemInstance, int newCount)
        {
            itemInstance._SetCount(newCount);
            if (itemInstance.Count <= 0) // if count is 0, remove from inventory
            {
                items.Remove(itemInstance);
            }

            InventoryUpdate?.Invoke();
        }

        /// <summary>
        /// Checks if the inventory contains the specified itemInstance
        /// </summary>
        /// <returns></returns>
        public bool Contains(ItemInstance itemInstance)
        {
            return items.Contains(itemInstance);
        }


        public int IndexOf(ItemInstance itemInstance)
        {
            if (itemInstance == null) return -1;
            return items.IndexOf(itemInstance);
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