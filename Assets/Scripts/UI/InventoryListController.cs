using Item;
using Player;
using UnityEngine;
using Utils;

namespace UI
{
    public class InventoryListController : MonoBehaviour
    {
        public Transform WeaponListContent;
        public GameObject WeaponListItemPrefab;
        public PlayerInventory playerInventory;
        private void Start()
        {
            playerInventory.inventoryUpdate += UpdateList;
        }

        void UpdateWeaponList()
        {
            WeaponListContent.DestroyChildren();
            foreach (WeaponItemInstance instance in playerInventory.GetAllItemOfType<WeaponItemInstance>())
            {
                GameObject item = Instantiate(WeaponListItemPrefab, WeaponListContent);
                item.GetComponent<InventoryListItemController>().SetItem(instance);
            }
        }

        void UpdateList()
        {
            UpdateWeaponList();
        }
    }
}