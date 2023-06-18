using Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class InventoryListItemController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public Image itemImage;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        private ItemInstance itemInstance;

        public void OnBeginDrag(PointerEventData eventData)
        {
            CursorController.GetInstance().StartDrag(itemInstance, itemInstance.itemType.itemSprite);
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            CursorController.GetInstance().EndDrag();
        }

        /// <summary>
        /// Sets the item instance this list item is showing
        /// </summary>
        /// <param name="item"></param>
        public void SetItem(ItemInstance item)
        {
            itemImage.sprite = item.itemType.itemSprite;
            title.text = item.GetDisplayName();
            description.text = item.GetDisplayDescription();
            itemInstance = item;
        }
    }
}