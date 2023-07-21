using System;
using Entities;
using Player;
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

        /// <summary>
        /// The inventory slot that this item is in.
        /// </summary>
        [HideInInspector]
        public InventorySlot slot;

        public Transform Player { get; private set; }
        public PlayerBody PlayerBody { get; private set; }

        private void Awake()
        {
            Player = GameObject.FindWithTag("Player").transform;
            PlayerBody = Player.GetComponent<PlayerBody>();
        }

        public void UseItem() => OnItemUsed?.Invoke();
        public void ReleaseItem() => OnItemReleased?.Invoke();
    }
}