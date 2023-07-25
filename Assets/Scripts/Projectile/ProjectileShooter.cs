using Core.Item;
using Item;
using UnityEngine;

namespace Projectile
{
    [RequireComponent(typeof(ItemPrefabController))]
    public class ProjectileShooter : MonoBehaviour
    {
        private ItemPrefabController gateway;
        public GameObject ProjectilePrefab;

        private void Start()
        {
            gateway = GetComponent<ItemPrefabController>();
            gateway.OnItemUsed += () =>
            {
                var projectile = Instantiate(ProjectilePrefab, gateway.Player.position, gateway.Player.rotation).GetComponent<Projectile>();
                projectile.projectileStats = gateway.slot.Item.itemInstance;
            };
        }
        

    }
}