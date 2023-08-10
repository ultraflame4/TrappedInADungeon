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
            if (!other.gameObject.CompareTag("Loot"))return;
            if (Player.Body.IsDead)return;
            Player.Inventory.AddItem(other.gameObject.GetComponent<DroppedLootItem>().itemInstance);
            Destroy(other.gameObject);
        }
    }
}