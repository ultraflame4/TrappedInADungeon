using System;
using Core.Item;
using PlayerScripts;
using UI.Inventory;
using UnityEngine;

namespace Loot
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DroppedLootItem : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        public ItemInstance itemInstance { get; private set; }
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = itemInstance.sprite;
        }

        public void SetItem(ItemInstance item)
        {
            itemInstance = item;
        }


    }
}