using System;
using System.Linq;
using Core.Item;
using Core.Utils;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Inventory
{
    public class InventoryPanel : MonoBehaviour
    {
        public Transform WeaponListContent;
        public Transform SkillListContent;
        public Transform ItemListContent;
        [FormerlySerializedAs("WeaponListItemPrefab")] public GameObject ListItemPrefab;
        public PlayerInventory playerInventory;
        private InventoryListItem[] listItems;
        public static InventoryPanel Instance { get; private set; }
        private void Awake()
        {
            playerInventory.InventoryUpdate += UpdateItemLists;
                    
            if (Instance != null)
            {
                Debug.LogError("Warning: multiple instances of InventoryPanel found! The static instance will be changed to this one!!!! This is probably not what you want!");
            }
            Instance = this;
        }

        void UpdateItemLists()
        {
            WeaponListContent.DestroyChildren();
            SkillListContent.DestroyChildren();
            ItemListContent.DestroyChildren();
            listItems = new InventoryListItem[playerInventory.AllItems.Count];
            for (var i = 0; i < playerInventory.AllItems.Count; i++)
            {
                var instance = playerInventory.AllItems[i];
                GameObject item;
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

                var listItem = item.GetComponent<InventoryListItem>();
                listItem.SetItem(instance);
                listItems[i] = listItem;
            }
        }


        public InvSlotItemInstance GetFocused()
        {
            if (!enabled) return null;
            return listItems.FirstOrDefault(x => x.itemInstance.focused)?.itemInstance;
        }
    }
}