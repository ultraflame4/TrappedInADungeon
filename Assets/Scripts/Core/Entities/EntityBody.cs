using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using Newtonsoft.Json;
using UnityEngine;

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

        public int baseAttack; // Increases physical damage

        public int BaseSpeed; // Movement speed
        public int BaseDefense; // Reduces damage taken

        [field: SerializeField]
        [JsonProperty]
        public VolatileValue<float> CurrentHealth { get; private set; } = new(); // Automatically set to Health on start

        [JsonProperty]
        protected List<StatsModifier> StatsModifiers = new();

        public float Health => BaseHealth * Level + StatsModifiers.Sum(modifier => modifier.Health);
        public float Attack => baseAttack * Level + StatsModifiers.Sum(modifier => modifier.Attack);
        public float Speed => BaseSpeed +  BaseSpeed * (Level-1) * .001f + StatsModifiers.Sum(modifier => modifier.Speed);
        public float Defense => BaseDefense * Level + StatsModifiers.Sum(modifier => modifier.Defense);

        [JsonProperty]
        public int Level = 1; // Level of entity

        public event Action DeathEvent; // Event that is invoked when entity dies
        public event Action DamagedEvent; // Event that is invoked when entity takes damage

        protected virtual void Awake()
        {
            CurrentHealth.value = Health;
            CurrentHealth.validator = (value, newValue) => Mathf.Min(Health, newValue);
        }

        public void Damage(float amt)
        {
            amt = Mathf.Min(1,amt * (1 - Defense/ (Defense+200)));
            CurrentHealth.value -= amt;
            DamagedEvent?.Invoke();
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
            return baseDamage + Attack;
        }
    }
}