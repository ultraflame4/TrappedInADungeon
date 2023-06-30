using Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryListItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public Image itemImage;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        private InventoryItemInstance itemInstance;

        /// <summary>
        /// Sets the item instance this list item is showing
        /// </summary>
        /// <param name="item"></param>
        public void SetItem(ItemInstance item)
        {
            itemImage.sprite = item.itemType.itemSprite;
            title.text = item.GetDisplayName();
            description.text = item.GetDisplayDescription();
            itemInstance = new InventoryItemInstance(item);
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CursorController.GetInstance().StartDrag(itemInstance,itemInstance.itemInstance.itemType.itemSprite);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            CursorController.GetInstance().EndDrag();
        }
    }
}