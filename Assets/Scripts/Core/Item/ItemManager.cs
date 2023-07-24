using System;
using System.Linq;
using UnityEngine;

namespace Core.Item
{
    public class ItemManager : MonoBehaviour
    {
        private ItemScriptableObject[] scriptableObjects;
        public static ItemManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Warning: multiple instances of ItemManager found! The static instance will be changed to this one!!!! This is probably not what you want!");
            }
            Instance = this;
            scriptableObjects = Resources.LoadAll<ItemScriptableObject>("Items");
        }

        

        public ItemScriptableObject FindItemById(string item_id)
        {
            return scriptableObjects.FirstOrDefault(x => x.item_id == item_id);
        }
    }
}