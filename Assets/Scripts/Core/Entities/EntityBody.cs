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
        protected List<AppliedStatsModifier> StatsModifiers = new();

        protected List<StatusEffect> StatusEffects = new();

        public float Health => BaseHealth * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.Health);
        public float Attack => BaseAttack * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.Attack);
        public float Speed => BaseSpeed + BaseSpeed * (Level - 1) * .001f + StatsModifiers.Sum(modifier => modifier.statsModifier.Speed);
        public float Defense => BaseDefense * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.Defense);


        [JsonProperty]
        public int Level = 1; // Level of entity

        public event Action DeathEvent; // Event that is invoked when entity dies
        public event Action<float> DamagedEvent; // Event that is invoked when entity takes damage

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
        /// Adds a status effect to this entity.<br/>
        /// Warning THIS ADDS A COPY OF THE INSTANCE OF THE STATUS EFFECT. TO REMOVE IT, USE THE INSTANCE RETURNED BY THIS METHOD.
        /// </summary>
        /// <param name="statusEffect"></param>
        /// <returns></returns>
        public StatusEffect AddStatusEffect(StatusEffect statusEffect)
        {
            // Create a new instance of the status effect (to avoid modifying the original)
            statusEffect = Instantiate(statusEffect);
            StatusEffects.Add(statusEffect);
            StatsModifiers.Add(new AppliedStatsModifier(statusEffect,statusEffect.statsOnce));
            return statusEffect;
        }
        /// <summary>
        /// Removes the specified status effect from this entity. Note: Use the instance returned by <see cref="AddStatusEffect"/> to remove the status effect.
        /// </summary>
        /// <param name="statusEffect"></param>
        private void RemoveStatusEffect(StatusEffect statusEffect)
        {
            StatusEffects.Remove(statusEffect);
            StatsModifiers.RemoveAll(modifier => (modifier.owner as StatusEffect) == statusEffect);
        }

        IEnumerator TickStatusEffect()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / ticksPerSecond);
                // Create shallow copy to avoid modification while iterating
                var copy = StatusEffects.GetRange(0, StatusEffects.Count);
                foreach (var statusEffect in copy)
                {
                    StatsModifiers.Add(new AppliedStatsModifier(statusEffect,statusEffect.statsOverTime));
                    statusEffect.ticks--;
                    if (statusEffect.ticks <= 0)
                    {
                        RemoveStatusEffect(statusEffect);
                    }
                }
            }
        }

        public void Damage(float amt)
        {
            amt *= Mathf.Min(0.1f, 1 - Defense / (Defense + 200));
            CurrentHealth.value -= amt;
            DamagedEvent?.Invoke(amt);
            if (CurrentHealth.value <= 0)
            {
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