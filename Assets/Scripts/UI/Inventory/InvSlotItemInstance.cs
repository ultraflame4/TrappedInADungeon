using System;
using Core.Item;
using Newtonsoft.Json;

namespace UI.Inventory
{
    /// <summary>
    /// Represents an instance of an item that is in the inventory.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InvSlotItemInstance
    {
        /// <summary>
        /// The item instance this is wrapping.
        /// </summary>
        public ItemInstance itemInstance;
        /// <summary>
        /// The slot this item is assigned to. Null if not assigned.
        /// </summary>
        public InventorySlot assignedSlot = null;
        /// <summary>
        /// Whether this item is focused in the inventory list panel ui thing.
        /// </summary>
        public bool focused = false;
        
        public InvSlotItemInstance(ItemInstance itemInstance)
        {
            this.itemInstance = itemInstance;
        }
        
    }
}