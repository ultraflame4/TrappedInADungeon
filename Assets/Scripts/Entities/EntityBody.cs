﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        public float CurrentHealth; // Automatically set to Health on start
        public float CurrentStamina; // Automatically set to Stamina on start
        public float CurrentMana; // Automatically set to Mana on start

        private List<IStatusEffect> statusEffects; // Current status effects on entity todo implement status effects
        /// <summary>
        /// The current status effects on this entity.
        /// </summary>
        public IStatusEffect[] StatusEffects => statusEffects.ToArray();
        
        public float Health => BaseHealth*Level;
        public float Stamina => BaseStamina*Level;
        public float Mana => BaseMana*Level;
        public float Attack => baseAttack*Level;
        public float Speed => BaseSpeed*Level;
        public float Defense => BaseDefense*Level;
        
        public int Level = 1; // Level of entity

        public event Action OnDeathEvent; // Event that is invoked when entity dies
        public event Action OnDamagedEvent; // Event that is invoked when entity takes damage
        void Start()
        {
            CurrentHealth = Health;
            CurrentStamina = Stamina;
            CurrentMana = Mana;
        }
        public void Damage(float amt)
        {
            CurrentHealth -= amt;
            OnDamagedEvent?.Invoke();
            if (CurrentHealth <= 0)
            {
                OnDeathEvent?.Invoke();
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
            return baseDamage+Attack;
        }

        public IStatusEffect AddStatusEffect(IStatusEffect statusEffect)
        {
            statusEffects.Add(statusEffect);
            statusEffect.Start(this);
            return statusEffect;
        }
        
        private void Update()
        {
            foreach (var statusEffect in statusEffects)
            {
                statusEffect.Tick(this);
            }
        }
    }
}