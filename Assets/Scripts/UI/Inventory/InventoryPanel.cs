using System;
using System.Linq;
using Core.Item;
using Core.Save;
using Core.Utils;
using Newtonsoft.Json;
using PlayerScripts;
using UnityEngine;

namespace UI.Inventory
{
    public class InventoryPanel : MonoBehaviour, ISaveHandler
    {
        [Tooltip("Scrollable content for the weapon list")]
        public Transform WeaponListContent;

        [Tooltip("Scrollable content for the skill list")]
        public Transform SkillListContent;

        [Tooltip("Scrollable content for the item list")]
        public Transform ItemListContent;

        [Tooltip("Prefab for the list item")]
        public GameObject ListItemPrefab;


        private InventoryListItem[] listItems;
        public static InventoryPanel Instance { get; private set; }

        private void Awake()
        {
            // Listen for inventory updates
            Player.Inventory.InventoryUpdate += UpdateItemLists;

            if (Instance != null)
            {
                Debug.LogError("Warning: multiple instances of InventoryPanel found! The static instance will be changed to this one!!!! This is probably not what you want!");
            }

            Instance = this;
        }

        void UpdateItemLists()
        {
            // On item list update, destroy all children and repopulate
            WeaponListContent.DestroyChildren();
            SkillListContent.DestroyChildren();
            ItemListContent.DestroyChildren();
            // New array to store inventory list items
            listItems = new InventoryListItem[Player.Inventory.AllItems.Count];
            // Loop over all items in the inventory and create a inventory list item for each
            for (var i = 0; i < Player.Inventory.AllItems.Count; i++)
            {
                var instance = Player.Inventory.AllItems[i];
                GameObject item;
                // Create the inventory list item with the correct parent according to the item type.
                switch (instance.item.itemType)
                {
                    case ItemType.Weapon:
                        item = Instantiate(ListItemPrefab, WeaponListContent);
                        break;
                    case ItemType.Skill:
                        item = Instantiate(ListItemPrefab, SkillListContent);
                        break;
                    default:
                        item = Instantiate(ListItemPrefab, ItemListContent);
                        break;
                }

                // Configure the list item with the correct item instance
                var listItem = item.GetComponent<InventoryListItem>();
                listItem.SetItem(instance);
                listItems[i] = listItem;
            }
        }


        /// <summary>
        /// Returns the currently focused item instance in the inventory
        /// </summary>
        /// <returns></returns>
        public InvSlotItemInstance GetFocused()
        {
            if (!enabled) return null;
            return listItems.FirstOrDefault(x => x.itemInstance.focused)?.itemInstance;
        }

        /// <summary>
        /// Returns the item instance at the given index in the inventory
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public InvSlotItemInstance GetInvItemByIndex(int itemIndex)
        {
            if (itemIndex < 0) return null;
            return listItems[itemIndex].itemInstance;
        }
    }
}