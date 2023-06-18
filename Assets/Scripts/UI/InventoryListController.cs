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

        private void UpdateWeaponList()
        {
            WeaponListContent.DestroyChildren();
            foreach (var instance in playerInventory.GetAllItemOfType<WeaponItemInstance>())
            {
                var item = Instantiate(WeaponListItemPrefab, WeaponListContent);
                item.GetComponent<InventoryListItemController>().SetItem(instance);
            }
        }

        private void UpdateList()
        {
            UpdateWeaponList();
        }
    }
}