
using UnityEngine;

namespace Item
{
    
    public interface IItemInstance
    {
        // public ItemScriptableObject itemType { get; }
        /// <summary>
        /// Returns the name to be shown in the inventory list.
        /// Can contain text mesh pro rich text
        /// </summary>
        /// <returns></returns>
        public string GetDisplayName();

        /// <summary>
        /// Returns the description to be shown in the inventory list
        /// Can contain text mesh pro rich text
        /// </summary>
        /// <returns></returns>
        public string GetDisplayDescription();
        public Sprite sprite { get; }
    }
}