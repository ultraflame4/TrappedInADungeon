using Item;

namespace UI.Inventory
{
    /// <summary>
    /// Represents an instance of an item that is in the inventory.
    /// 
    /// </summary>
    public class InvSlotItemInstance
    {
        public ItemInstance itemInstance;
        public InventorySlot assignedSlot = null;
        public bool focused = false;
        public InvSlotItemInstance(ItemInstance itemInstance)
        {
            this.itemInstance = itemInstance;
        }
    }
}