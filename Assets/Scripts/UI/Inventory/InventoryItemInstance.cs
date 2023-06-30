using Item;

namespace UI.Inventory
{
    /// <summary>
    /// Represents an instance of an item that is in the inventory.
    /// 
    /// </summary>
    public class InventoryItemInstance
    {
        public ItemInstance itemInstance;
        public InventorySlot assignedSlot = null;
        public InventoryItemInstance(ItemInstance itemInstance)
        {
            this.itemInstance = itemInstance;
        }
    }
}