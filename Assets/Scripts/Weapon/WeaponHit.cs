﻿using System;
using Core.Entities;
using Core.Sound;
using Entities;
using PlayerScripts;
using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(WeaponController))]
    public class WeaponHit : MonoBehaviour
    {
        [SerializeField]
        private WeaponController controller;
        

        private void Start()
        {
            controller = GetComponent<WeaponController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {

            if (!other.gameObject.CompareTag("Enemy")) return;
            EntityBody body = other.gameObject.GetComponent<EntityBody>();
            if (body is null) return;
            body.Damage(Player.Body.CalculateAttackDamage(controller.weaponItem.Attack));
        }
        
    }
}