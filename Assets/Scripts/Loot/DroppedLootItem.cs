using System;
using Core.Item;
using Player;
using UI.Inventory;
using UnityEngine;

namespace Loot
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DroppedLootItem : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private ItemInstance itemInstance;
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = itemInstance.sprite;
        }

        public void SetItem(ItemInstance item)
        {
            itemInstance = item;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("Player")) return;
            other.collider.GetComponent<PlayerInventory>().AddItem(itemInstance);
            Destroy(gameObject);
        }
    }
}