using System;
using UI.Inventory;
using UnityEngine;

namespace Item
{
    /// <summary>
    /// This component is used to communicate between the item prefab and the hotbar.
    /// </summary>
    public class ItemPrefabHotbarGateway : MonoBehaviour
    {
        /// <summary>
        /// When player use the item (aka press the keybind for the hotbar with this item)
        /// </summary>
        public event Action OnItemUsed;
        /// <summary>
        /// When player release the item (aka release the keybind for the hotbar with this item)
        /// </summary>
        public event Action OnItemReleased;

        public InventorySlot slot;
        
        public void UseItem() => OnItemUsed?.Invoke();
        public void ReleaseItem() => OnItemReleased?.Invoke();
    }
}