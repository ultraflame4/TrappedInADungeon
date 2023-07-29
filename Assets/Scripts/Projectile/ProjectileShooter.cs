using Core.Item;
using Item;
using PlayerScripts;
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
                var projectile = Instantiate(ProjectilePrefab, Player.Transform.position, Player.Transform.rotation).GetComponent<Projectile>();
                projectile.projectileStats = gateway.slot.Item.itemInstance;
            };
        }
        

    }
}