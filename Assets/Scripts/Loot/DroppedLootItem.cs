using System;
using Core.Item;
using PlayerScripts;
using UI.Inventory;
using UnityEngine;

namespace Loot
{
    /// <summary>
    /// Controller for items dropped on the ground
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class DroppedLootItem : MonoBehaviour
    {
        // component reference
        private SpriteRenderer spriteRenderer;
        /// <summary>
        /// The item this will give to the player
        /// </summary>
        public ItemInstance itemInstance { get; private set; }
        private void Start()
        {
            // Get component reference
            spriteRenderer = GetComponent<SpriteRenderer>();
            // Set sprite of the prefab to the item sprite
            spriteRenderer.sprite = itemInstance.sprite;
        }
        
        /// <summary>
        /// Used to set the item instance for this dropped item prefab
        /// </summary>
        /// <param name="item"></param>
        public void SetItem(ItemInstance item)
        {
            itemInstance = item;
        }


    }
}