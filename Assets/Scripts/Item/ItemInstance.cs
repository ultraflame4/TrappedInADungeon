namespace Item
{
    
    public class ItemInstance
    {
        public ItemScriptableObject itemType { get; }
        public ItemInstance(ItemScriptableObject itemType)
        {
            this.itemType = itemType;
        }

        /// <summary>
        /// Returns the name to be shown in the inventory list.
        /// Can contain text mesh pro rich text
        /// </summary>
        /// <returns></returns>
        public virtual string GetDisplayName()
        {
            return itemType.name;
        }
        /// <summary>
        /// Returns the description to be shown in the inventory list
        /// Can contain text mesh pro rich text
        /// </summary>
        /// <returns></returns>
        public virtual string GetDisplayDescription()
        {
            return itemType.item_description;
        }
    }

    public class WeaponItemInstance : ItemInstance // Convenience class so that we don't have to check and cast itemType to WeaponItem every time
    {
        public WeaponItem weaponType => (WeaponItem)itemType;
        public WeaponItemInstance(WeaponItem itemType) : base(itemType) { }
    }
}