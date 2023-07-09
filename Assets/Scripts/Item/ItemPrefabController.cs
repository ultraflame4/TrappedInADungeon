using System;
using UI.Inventory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Item
{
    /// <summary>
    /// This component is used to communicate between the item prefab and the hotbar.
    /// </summary>
    public class ItemPrefabController : MonoBehaviour
    {
        /// <summary>
        /// When player use the item (aka press the keybind for the hotbar with this item)
        /// </summary>
        public event Action OnItemUsed;
        /// <summary>
        /// When player release the item (aka release the keybind for the hotbar with this item)
        /// </summary>
        public event Action OnItemReleased;

        private void Awake()
        {
            Player = GameObject.FindWithTag("Player").transform;
        }

        public InventorySlot slot;
        [FormerlySerializedAs("player")] public Transform Player;

        public void UseItem() => OnItemUsed?.Invoke();
        public void ReleaseItem() => OnItemReleased?.Invoke();
    }
}