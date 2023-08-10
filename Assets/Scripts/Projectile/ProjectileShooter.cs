using Item;
using PlayerScripts;
using UnityEngine;

namespace Projectile
{
    /// <summary>
    /// This component shoots a projectile when the item is used.
    /// </summary>
    [RequireComponent(typeof(ItemPrefabController))]
    public class ProjectileShooter : MonoBehaviour
    {
        // reference to the item prefab controller
        private ItemPrefabController gateway;

        [Tooltip("Projectile prefab to shoot")]
        public GameObject ProjectilePrefab;

        private void Start()
        {
            // Register event and stuff...
            gateway = GetComponent<ItemPrefabController>();
            gateway.OnItemUsed += () =>
            {
                // Spawn the projectile
                var projectile = Instantiate(ProjectilePrefab, Player.Transform.position, Player.Transform.rotation).GetComponent<Projectile>();
                // Set the projectile stats to the item instance
                projectile.projectileStats = gateway.slot.Item.itemInstance;
            };
        }
        

    }
}