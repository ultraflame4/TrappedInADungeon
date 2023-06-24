﻿namespace Item
{
    
    public class ItemInstance
    {
        public ItemScriptableObject itemType;
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
}