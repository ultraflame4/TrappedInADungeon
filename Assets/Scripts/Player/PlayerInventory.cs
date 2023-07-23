using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Item;
using Core.Save;
using EasyButtons;
using Newtonsoft.Json;
using UI.Inventory;
using UnityEngine;
using Newtonsoft.Json.Linq;
namespace Player
{
    public class PlayerInventory : MonoBehaviour, ISaveHandler
    {
        [SerializeField]
        private List<ItemInstance> items = new ();

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

            GameSaveManager.AddSaveHandler("player.inventory",this);
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
        /// Adds an item instance to the inventory. Note that this will also automatically try to combine the itemInstance to a similar itemInstance.
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public void AddItem<T>(T item) where T : ItemInstance
        {
            // Search through all items
            foreach (ItemInstance itemInstance in AllItems)
            {
                if (itemInstance.Combine(item)) 
                {
                    // If current item combines successfully, invoke event & return
                    InventoryUpdate?.Invoke();
                    return;
                }
            }
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
        /// Adjusts number of items in the specified itemInstance & invokes inventory update event
        /// </summary>
        /// <param name="itemInstance"></param>
        /// <param name="newCount"></param>
        public void AdjustItemCount(ItemInstance itemInstance,int newCount)
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

        public void OnLoadSave(string json)
        {
            var array = JArray.Parse(json);
            List<ItemInstance> savedItems = new (); 
            foreach (JToken child in array.Children())
            {
                ItemInstance itemInstance = new (null);
                JsonUtility.FromJsonOverwrite(child.ToString(),itemInstance);
                savedItems.Add(itemInstance);
            }

            if (savedItems.Count > 0)
            {
                items = savedItems;
            }
        }

        public string OnWriteSave()
        {
            var jsonObj = new JArray();
            foreach (ItemInstance item in AllItems)
            {
                // Use JsonUtility to serialise unity objects, before parsing the string result into a JObject and adding it to the JArray
                // Not really efficient but wtv.
                jsonObj.Add(JObject.Parse(JsonUtility.ToJson(item)));
            }

            return jsonObj.ToString();
        }
    }
}