using System;
using Item;
using UnityEngine;

namespace Skills
{
    public class ProjectileShooter : MonoBehaviour
    {
        
        public ItemPrefabController Gateway;
        public GameObject ProjectilePrefab;

        private void Start()
        {
            Gateway.OnItemUsed += () =>
            {
                var projectile = Instantiate(ProjectilePrefab, Gateway.Player.position, Gateway.Player.rotation).GetComponent<Projectile>();
                projectile.itemInstance = Gateway.slot.Item.itemInstance;
            };
        }

    }
}