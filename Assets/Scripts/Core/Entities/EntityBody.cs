using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Entities
{
    /// <summary>
    /// This class represents the "body" / (virtual) physical conditions of an entity,
    /// such as its health, stamina, strength etc.
    /// Note that the values stored by the variables/attributes with _ prefixed only store the base values, it does not include effects from equipment, buffs, etc.
    /// To get the actual values, use the properties or Get methods. 
    /// </summary>
    public class EntityBody : MonoBehaviour, IEntityStats
    {
        public float BaseHealth; // Base Health of entity
        public int BaseAttack; // Base attack damage of entity
        public int BaseSpeed; // Base Movement speed
        public int BaseDefense; // Reduces damage taken

        [Tooltip("Ticks affect status effects duration and updates. The more tick per second, the shorter the duration.")]
        public int ticksPerSecond = 2;

        [field: SerializeField]
        [JsonProperty] // Mark this as serializable so that it can be serialised in the child class : PlayerBody
        public VolatileValue<float> CurrentHealth { get; private set; } = new(); // Automatically set to Health on start

        [JsonProperty] // Mark this as serializable so that it can be serialised in the child class : PlayerBody
        public List<AppliedStatsModifier> StatsModifiers { get; private set; } = new();

        public List<ActiveStatusEffect> StatusEffects { get; private set; } = new();

        // ---- Final stats of entity -----
        // Calculated with this Formula: BaseStat * Level + Sum of all modifiers
        public float Health => BaseHealth * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.Health);
        public float Attack => BaseAttack * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.Attack);
        // Formula for speed is a bit different else it would be too fast
        // Clamp to 0 to prevent negative speed
        public float Speed => Mathf.Max(0,BaseSpeed + BaseSpeed * (Level - 1) * .001f + StatsModifiers.Sum(modifier => modifier.statsModifier.Speed));
        public float Defense => BaseDefense * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.Defense);


        [JsonProperty] // Mark this as serializable so that it can be serialised in the child class : PlayerBody
        public int Level = 1; // Level of entity

        public event Action DeathEvent; // Event that is invoked when entity dies
        public bool IsDead { get; private set; } = false;

        /// <summary>
        /// Set this to true to make the entity invulnerable to damage.
        /// </summary>
        public bool invulnerable = false;

        public delegate void OnDamagedHandler(float amt, bool stun); // Event handler for when entity takes damage

        public event OnDamagedHandler DamagedEvent; // Event that is invoked when entity takes damage

        protected virtual void Awake()
        {
            // Intercepts the value change of CurrentHealth to make sure it doesn't go over Health
            CurrentHealth.validator = (value, newValue) => Mathf.Min(Health, newValue);
        }

        protected virtual void Start()
        {
            // Set current health to max health
            CurrentHealth.value = Health;
            // Start coroutine for ticking status effects
            StartCoroutine(TickStatusEffect());
        }

        /// <summary>
        /// Adds a status effect to this entity.
        /// </summary>
        /// <param name="statusEffect"></param>
        /// <returns></returns>
        public ActiveStatusEffect AddStatusEffect(StatusEffect statusEffect)
        {
            var particle = Instantiate(statusEffect.particleEffect, transform); // Instantiate particle effect
            var active = new ActiveStatusEffect(statusEffect, particle); // Create active status effect
            StatusEffects.Add(active); // Add to list of active status effects
            active.TickStart(this); // Call start event
            return active;
        }

        private IEnumerator TickStatusEffect()
        {
            while (true)
            {
                // Wait for 1/ticksPerSecond seconds
                yield return new WaitForSeconds(1f / ticksPerSecond);
                // Create shallow copy to avoid modification while iterating
                var copy = StatusEffects.GetRange(0, StatusEffects.Count);
                // Loop through all status effects and update them
                foreach (var activeStatusEffect in copy)
                {
                    activeStatusEffect.Tick(this);
                    if (activeStatusEffect.ticksRemaining <= 0) // If status effect has expired, remove it
                    {
                        activeStatusEffect.Remove(this);
                    }
                }
            }
        }

        /// <summary>
        /// Deals damage to the entity. Damage is reduced by the entity's defense.
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="stun">Whether to stun enemy when dealing damage</param>
        public void Damage(float amt, bool stun = true)
        {
            if (IsDead) return; // If dead, don't take damage
            amt *= Mathf.Min(0.1f, 1 - Defense / (Defense + 200)); // Fancy math to reduce damage by defense
            DamageRaw(amt, stun);
        }

        /// <summary>
        /// Deals damage directly to the entity's health, bypassing defense.
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="stun">Whether to stun enemy when dealing damage</param>
        public void DamageRaw(float amt, bool stun = true)
        {
            if (invulnerable) return; // If invulnerable, don't take damage
            CurrentHealth.value -= amt;
            DamagedEvent?.Invoke(amt, stun);
            if (CurrentHealth.value <= 0 && !IsDead) // If health is lesser than 0, set entity to dead
            {
                IsDead = true;
                DeathEvent?.Invoke();
            }
        }


        /// <summary>
        /// Calculates the damage to be dealt by an attack based on the current stats of this entity.
        /// </summary>
        /// <param name="baseDamage"></param>
        /// <returns></returns>
        public float CalculateAttackDamage(float baseDamage)
        {
            return baseDamage * Level + Attack;
        }
    }
}