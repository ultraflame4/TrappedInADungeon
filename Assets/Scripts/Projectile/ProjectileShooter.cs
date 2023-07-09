using Item;
using UnityEngine;

namespace Projectile
{
    public class ProjectileShooter : MonoBehaviour
    {
        
        public ItemPrefabController Gateway;
        public GameObject ProjectilePrefab;

        private void Start()
        {
            Gateway.OnItemUsed += () =>
            {
                var projectile = Instantiate(ProjectilePrefab, Gateway.Player.position, Gateway.Player.rotation).GetComponent<global::Projectile.Projectile>();
                projectile.projectileStats = Gateway.slot.Item.itemInstance;
            };
        }

    }
}