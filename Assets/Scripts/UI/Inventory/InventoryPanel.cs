using System.Linq;
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
        [FormerlySerializedAs("WeaponListItemPrefab")] public GameObject ListItemPrefab;
        public PlayerInventory playerInventory;
        private void Awake()
        {
            playerInventory.InventoryUpdate += UpdateList;
        }

        void UpdateWeaponList()
        {
            WeaponListContent.DestroyChildren();
            foreach (IItemInstance instance in playerInventory.GetAllItemOfType<WeaponItem>())
            {
                GameObject item = Instantiate(ListItemPrefab, WeaponListContent);
                item.GetComponent<InventoryListItem>().SetItem(instance);
            }
        }
        void UpdateSkillList()
        {
            SkillListContent.DestroyChildren();
            foreach (IItemInstance instance in playerInventory.GetAllItemOfType<SkillItem>())
            {
                GameObject item = Instantiate(ListItemPrefab, SkillListContent);
                item.GetComponent<InventoryListItem>().SetItem(instance);
            }
        }

        void UpdateList()
        {
            UpdateWeaponList();
            UpdateSkillList();
        }
        
        void Update()
        {

        }
    }
}