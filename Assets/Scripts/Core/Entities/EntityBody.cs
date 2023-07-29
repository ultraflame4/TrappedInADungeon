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
    /// This class represents the "body" / (virtual)physical conditions of an entity,
    /// such as its health, stamina, strength etc.
    /// Note that the values stored by the variables/attributes with _ prefixed only store the base values, it does not include effects from equipment, buffs, etc.
    /// To get the actual values, use the properties or Get methods. 
    /// </summary>
    public class EntityBody : MonoBehaviour, IEntityStats
    {
        public float BaseHealth; // Health of entity

        [FormerlySerializedAs("baseAttack")]
        public int BaseAttack; // Increases physical damage

        public int BaseSpeed; // Movement speed
        public int BaseDefense; // Reduces damage taken

        [Tooltip("Ticks affect status effects duration and updates. The more tick per second, the shorter the duration.")]
        public int ticksPerSecond = 2;

        [field: SerializeField]
        [JsonProperty]
        public VolatileValue<float> CurrentHealth { get; private set; } = new(); // Automatically set to Health on start

        [JsonProperty]
        public List<AppliedStatsModifier> StatsModifiers { get; private set; } = new();

        public List<ActiveStatusEffect> StatusEffects { get; private set; } = new();

        public float Health => BaseHealth * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.Health);
        public float Attack => BaseAttack * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.Attack);
        public float Speed => BaseSpeed + BaseSpeed * (Level - 1) * .001f + StatsModifiers.Sum(modifier => modifier.statsModifier.Speed);
        public float Defense => BaseDefense * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.Defense);


        [JsonProperty]
        public int Level = 1; // Level of entity
        
        public event Action DeathEvent; // Event that is invoked when entity dies
        public bool IsDead { get; private set; } = false;

        public delegate void OnDamagedHandler(float amt, bool stun); // Event handler for when entity takes damage

        public event OnDamagedHandler DamagedEvent; // Event that is invoked when entity takes damage

        protected virtual void Awake()
        {
            CurrentHealth.validator = (value, newValue) => Mathf.Min(Health, newValue);
        }

        protected virtual void Start()
        {
            CurrentHealth.value = Health;
            StartCoroutine(TickStatusEffect());
        }

        /// <summary>
        /// Adds a status effect to this entity.
        /// </summary>
        /// <param name="statusEffect"></param>
        /// <returns></returns>
        public ActiveStatusEffect AddStatusEffect(StatusEffect statusEffect)
        {
            // Create a new instance of the status effect (to avoid modifying the original)
            var particle = Instantiate(statusEffect.particleEffect, transform);
            var active = new ActiveStatusEffect(statusEffect, particle);
            StatusEffects.Add(active);
            active.TickStart(this);

            return active;
        }

        IEnumerator TickStatusEffect()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / ticksPerSecond);
                // Create shallow copy to avoid modification while iterating
                var copy = StatusEffects.GetRange(0, StatusEffects.Count);
                foreach (var activeStatusEffect in copy)
                {
                    activeStatusEffect.Tick(this);
                    if (activeStatusEffect.ticksRemaining <= 0)
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
            amt *= Mathf.Min(0.1f, 1 - Defense / (Defense + 200));
            DamageRaw(amt, stun);
        }

        /// <summary>
        /// Deals damage directly to the entity's health, bypassing defense.
        /// </summary>
        /// <param name="amt"></param>
        /// <param name="stun">Whether to stun enemy when dealing damage</param>
        public void DamageRaw(float amt, bool stun = true)
        {
            CurrentHealth.value -= amt;
            DamagedEvent?.Invoke(amt, stun);
            if (CurrentHealth.value <= 0 && !IsDead)
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