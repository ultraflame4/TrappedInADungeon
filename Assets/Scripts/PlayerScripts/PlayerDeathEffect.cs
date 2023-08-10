using System.Collections;
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
        [Tooltip("The shadow effect prefab thing to spawn when the player dies")]
        public GameObject DeadShadowPrefab;
        [Tooltip("The dropped item prefab (when player dies, drop all items)")]
        public GameObject droppedItemPrefab;
        // Component references
        private PlayerBody body;
        private PlayerInventory inventory;
        private SpriteRenderer spriteRenderer;
        private Movement movement;
        private void Start()
        {
            // Get the references
            body = Player.Body;
            inventory = Player.Inventory;
            movement = GetComponent<Movement>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            // Register listener for death event.
            body.DeathEvent += OnDeath;
        }
        


        private void OnDeath()
        {
            // Instantiate the shadow thing to spawn
            var shadow = Instantiate(DeadShadowPrefab, transform.position, transform.rotation);
            shadow.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite; // Set shadow sprite to the sprite of this entity
            // push helpful notifications
            NotificationManager.Instance.PushNotification("<color=#f00><size=150%>You died!</size></color>");
            // When player dies, disabled movement and hide player sprite
            movement.enabled = false;
            spriteRenderer.enabled = false;
            // Restore player health so on respawn, the respawn with full health
            body.CurrentHealth.value = body.Health;
            // Drop all items in inventory
            foreach (ItemInstance item in inventory.AllItems)
            {
                DroppedLootItem lootItem = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity).GetComponent<DroppedLootItem>();
                // Give dropped item some velocity to make more interesting :D
                lootItem.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f,1f), Random.value);
                lootItem.SetItem(item);
                inventory.RemoveItem(item);
            }
            inventory.ResetInventory(); // Reset the player inventory to default items
        }
    }
}