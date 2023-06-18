using Item;
using TMPro;
using UI.Dragging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class InventoryListItemController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public Image itemImage;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        private ItemInstance itemInstance;
        
        /// <summary>
        /// Sets the item instance this list item is showing
        /// </summary>
        /// <param name="item"></param>
        public void SetInstance(ItemInstance item)
        {
            itemImage.sprite = item.itemType.itemSprite;
            title.text = item.GetDisplayName();
            description.text = item.GetDisplayDescription();
            itemInstance = item;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CursorController.GetInstance().SetDragEventData(new InventoryDragEventData(itemInstance));
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            CursorController.GetInstance().SetDragEventData(null);
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}