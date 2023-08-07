using System;
using System.Linq;
using UnityEngine;

namespace Core.Item
{
    /// <summary>
    /// Keeps track of all items in the game.
    /// </summary>
    public class ItemManager : MonoBehaviour
    {
        /// <summary>
        /// An array of all items in the game.
        /// </summary>
        private ItemScriptableObject[] scriptableObjects;
        public static ItemManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Warning: multiple instances of ItemManager found! The static instance will be changed to this one!!!! This is probably not what you want!");
            }
            Instance = this;
            // Find all items in the resources folder.
            scriptableObjects = Resources.LoadAll<ItemScriptableObject>("Items");
        }
        
        /// <summary>
        /// Returns an item by its id.
        /// </summary>
        /// <param name="item_id">id of target item</param>
        /// <returns></returns>
        public ItemScriptableObject FindItemById(string item_id)
        {
            // Find the item with the matching id, else return null.
            return scriptableObjects.FirstOrDefault(x => x.item_id == item_id);
        }
    }
}