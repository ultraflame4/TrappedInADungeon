using Core.Item;
using Core.UI;
using Loot;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(Movement)),RequireComponent(typeof(SpriteRenderer)),RequireComponent(typeof(PlayerBody))]
    public class PlayerDeathEffect : MonoBehaviour
    {
        public GameObject DeadShadowPrefab;
        public GameObject droppedItemPrefab;
        private PlayerBody body;
        private PlayerInventory inventory;
        private SpriteRenderer spriteRenderer;
        private Movement movement;
        private void Start()
        {
            body = Player.Body;
            inventory = Player.Inventory;
            movement = GetComponent<Movement>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            body.DeathEvent += OnDeath;
        }

        private void OnDeath()
        {
            var shadow = Instantiate(DeadShadowPrefab, transform.position, transform.rotation);
            shadow.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite; // Set shadow sprite to the sprite of this entity
            NotificationManager.Instance.PushNotification("<color=#f00><size=150%>You died!</size></color>");
            movement.enabled = false;
            spriteRenderer.enabled = false;
            body.CurrentHealth.value = body.Health;
            foreach (ItemInstance item in inventory.AllItems)
            {
                DroppedLootItem lootItem = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity).GetComponent<DroppedLootItem>();
                lootItem.SetItem(item);
                inventory.RemoveItem(item);
            }
            
        }
    }
}