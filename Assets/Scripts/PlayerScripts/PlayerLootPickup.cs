using System;
using Loot;
using UnityEngine;

namespace PlayerScripts
{
    /// <summary>
    /// Adds picking up of loot item to player
    /// </summary>
    public class PlayerLootPickup : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            // When player collides with a dropped item, add to player inventory and destroy the dropped item
            
            if (!other.gameObject.CompareTag("Loot")) return; // if not loot, ignore
            if (Player.Body.IsDead) return; // if player is dead, ignore
            // add item to inventory and destroy the dropped item
            Player.Inventory.AddItem(other.gameObject.GetComponent<DroppedLootItem>().itemInstance);
            Destroy(other.gameObject);
        }
    }
}