using Item;
using UnityEngine;
using Utils;

namespace UI.Inventory
{
    public class InventoryPanel : MonoBehaviour
    {
        public Transform WeaponListContent;
        public GameObject WeaponListItemPrefab;
        public PlayerInventory playerInventory;
        private void Awake()
        {
            playerInventory.inventoryUpdate += UpdateList;
        }

        void UpdateWeaponList()
        {
            WeaponListContent.DestroyChildren();
            foreach (IItemInstance instance in playerInventory.GetAllItemOfType<WeaponItem>())
            {
                GameObject item = Instantiate(WeaponListItemPrefab, WeaponListContent);
                item.GetComponent<InventoryListItem>().SetItem(instance);
            }
        }

        void UpdateList()
        {
            UpdateWeaponList();
        }
        
        void Update()
        {

        }
    }
}