using System;
using Entities;
using UnityEngine;

namespace Weapon
{
    public class WeaponHit : MonoBehaviour
    {
        public WeaponController controller;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;
            EntityBody body = other.gameObject.GetComponent<EntityBody>();
            if (body is null) return;
            body.Damage(controller.weaponItem.Attack);
        }
        
    }
}