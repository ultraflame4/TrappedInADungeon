using System;
using Core.Entities;
using Core.Sound;
using Entities;
using PlayerScripts;
using UnityEngine;

namespace Weapon
{
    /// <summary>
    /// Applies weapon damage to enemies
    /// </summary>
    [RequireComponent(typeof(WeaponController))]
    public class WeaponHit : MonoBehaviour
    {
        // WeaponController reference;
        [SerializeField]
        private WeaponController controller;
        
        private void Start()
        {
            // Get component references
            controller = GetComponent<WeaponController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        { 
            // If not enemy return
            if (!other.gameObject.CompareTag("Enemy")) return;
            // Get enemy entity body and apply damage
            EntityBody body = other.gameObject.GetComponent<EntityBody>();
            // If body not found skip
            if (body is null) return;
            // Apply damage
            body.Damage(Player.Body.CalculateAttackDamage(controller.weaponItem.Attack));
        }
        
    }
}