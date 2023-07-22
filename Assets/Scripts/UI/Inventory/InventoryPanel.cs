﻿using System.Linq;
using Item;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace UI.Inventory
{
    public class InventoryPanel : MonoBehaviour
    {
        public Transform WeaponListContent;
        public Transform SkillListContent;
        public Transform ItemListContent;
        [FormerlySerializedAs("WeaponListItemPrefab")] public GameObject ListItemPrefab;
        public PlayerInventory playerInventory;
        private void Awake()
        {
            playerInventory.InventoryUpdate += UpdateList;
        }

        void UpdateWeaponList()
        {
            WeaponListContent.DestroyChildren();
            foreach (ItemInstance instance in playerInventory.GetAllItemOfType(ItemType.Weapon))
            {
                GameObject item = Instantiate(ListItemPrefab, WeaponListContent);
                item.GetComponent<InventoryListItem>().SetItem(instance);
            }
        }
        void UpdateSkillList()
        {
            SkillListContent.DestroyChildren();
            foreach (ItemInstance instance in playerInventory.GetAllItemOfType(ItemType.Skill))
            {
                GameObject item = Instantiate(ListItemPrefab, SkillListContent);
                item.GetComponent<InventoryListItem>().SetItem(instance);
            }
        }
        void UpdateItemList()
        {
            ItemListContent.DestroyChildren();
            foreach (ItemInstance instance in playerInventory.AllItems)
            {
                // Ignore weapon and skill items as they have their own section.
                if (instance.item.itemType == ItemType.Weapon || instance.item.itemType == ItemType.Skill) continue;
                GameObject item = Instantiate(ListItemPrefab, ItemListContent);
                item.GetComponent<InventoryListItem>().SetItem(instance);
            }
        }

        void UpdateList()
        {
            UpdateWeaponList();
            UpdateSkillList();
            UpdateItemList();
        }
        
        void Update()
        {

        }
    }
}