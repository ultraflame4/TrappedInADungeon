using Core.Entities;
using Entities;
using Player;
using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(WeaponController))]
    public class WeaponHit : MonoBehaviour
    {
        private PlayerBody playerBody;
        private WeaponController controller;
        private void Start()
        {
            playerBody = GameObject.FindWithTag("Player").GetComponent<PlayerBody>();
            controller = GetComponent<WeaponController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;
            EntityBody body = other.gameObject.GetComponent<EntityBody>();
            if (body is null) return;
            body.Damage(playerBody.CalculateAttackDamage(controller.weaponItem.Attack));
        }
        
    }
}