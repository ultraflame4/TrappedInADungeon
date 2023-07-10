﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Entities
{
    /// <summary>
    /// This class represents the "body" / (virtual)physical conditions of an entity,
    /// such as its health, stamina, strength etc.
    /// Note that the values stored by the variables/attributes with _ prefixed only store the base values, it does not include effects from equipment, buffs, etc.
    /// To get the actual values, use the properties or Get methods. 
    /// </summary>
    public class EntityBody : MonoBehaviour, IEntityStats
    {
        public float BaseHealth; // Health of entity
        public int BaseStamina; // Used for dashing
        public int BaseMana; // Used for casting spells
        [FormerlySerializedAs("BaseStrength")] public int baseAttack; // Increases physical damage
        public int BaseSpeed; // Movement speed
        public int BaseDefense; // Reduces damage taken

        private float _currentHealth; // Automatically set to Health on start

        public VolatileValue<float> CurrentHealth = new(); // Automatically set to Health on start
        public VolatileValue<float> CurrentStamina = new(); // Automatically set to Stamina on start
        public VolatileValue<float> CurrentMana = new(); // Automatically set to Mana on start


        private List<StatsModifier> StatsModifiers = new();
        public float Health => BaseHealth * Level + StatsModifiers.Sum(modifier => modifier.Health);
        public float Stamina => BaseStamina * Level + StatsModifiers.Sum(modifier => modifier.Stamina);
        public float Mana => BaseMana * Level + StatsModifiers.Sum(modifier => modifier.Mana);
        public float Attack => baseAttack * Level + StatsModifiers.Sum(modifier => modifier.Attack);
        public float Speed => BaseSpeed * Level + StatsModifiers.Sum(modifier => modifier.Speed);
        public float Defense => BaseDefense * Level + StatsModifiers.Sum(modifier => modifier.Defense);

        public int Level = 1; // Level of entity

        public event Action DeathEvent; // Event that is invoked when entity dies
        public event Action DamagedEvent; // Event that is invoked when entity takes damage

        void Start()
        {
            CurrentHealth.value = Health;
            CurrentStamina.value = Stamina;
            CurrentMana.value = Mana;
        }

        public void Damage(float amt)
        {
            CurrentHealth.value -= amt;
            DamagedEvent?.Invoke();
            if (CurrentHealth.value <= 0)
            {
                DeathEvent?.Invoke();
            }
        }
        // todo include methods to get the actual values that includes effects from equipment, buffs, etc.

        /// <summary>
        /// Calculates the damage to be dealt by an attack based on the current stats of this entity.
        /// </summary>
        /// <param name="baseDamage"></param>
        /// <returns></returns>
        public float CalculateAttackDamage(float baseDamage)
        {
            return baseDamage + Attack;
        }
    }
}