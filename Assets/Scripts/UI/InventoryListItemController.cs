using System.Collections.Generic;
using System.Linq;
using Item;
using TMPro;
using UI.Dragging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class InventoryListItemController : DraggableItem<InventorySlot>
    {
        public Image itemImage;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        private ItemInstance itemInstance;

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

        public override Sprite GetDragCursorSprite()
        {
            return itemInstance.itemType.itemSprite;
        }

        public override void DropOnSlot(InventorySlot slot)
        {
            slot.SetItem(itemInstance);
        }
    }
}