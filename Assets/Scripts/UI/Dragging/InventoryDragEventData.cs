using Item;
using UnityEngine;

namespace UI.Dragging
{
    public class InventoryDragEventData : DragEventData
    {
        public readonly ItemInstance itemInstance;
        public InventoryDragEventData(ItemInstance itemInstance)
        {
            this.itemInstance = itemInstance;
        }
        public override Sprite GetSprite() => itemInstance.itemType.itemSprite;
    }
}