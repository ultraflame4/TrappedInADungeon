using System;
using Item;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class InventoryListController : MonoBehaviour
    {
        public Transform WeaponListContent;
        public GameObject WeaponListItemPrefab;
        public PlayerController player;
        private void Start()
        {
            player.Inventory.inventoryUpdate += UpdateList;
        }

        void UpdateWeaponList()
        {
            WeaponListContent.DestroyChildren();
            foreach (WeaponItemInstance instance in player.Inventory.GetAllItemOfType<WeaponItemInstance>())
            {
                GameObject item = Instantiate(WeaponListItemPrefab, WeaponListContent);
                item.transform.Find("itemSprite").GetComponent<Image>().sprite = instance.itemType.itemSprite;
                item.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = instance.itemType.item_name;
                item.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = instance.itemType.item_description;
            }
        }

        void UpdateList()
        {
            UpdateWeaponList();
            
        }
    }
}