﻿using UnityEngine;

namespace Entities
{
    /// <summary>
    /// This class represents the "body" / (virtual)physical conditions of an entity,
    /// such as its health, stamina, strength etc.
    /// Note that the values stored by the variables/attributes with _ prefixed only store the base values, it does not include effects from equipment, buffs, etc.
    /// To get the actual values, use the properties or Get methods. 
    /// </summary>
    public class EntityBody : MonoBehaviour
    {
        public float BaseHealth; // Health of entity
        public int BaseStamina; // Used for dashing
        public int BaseMana; // Used for casting spells
        public int BaseStrength; // Increases physical damage
        public int BaseSpeed; // Movement speed
        public int BaseDefense; // Reduces damage taken
        
        public float CurrentHealth; // Automatically set to _Health on start
        public float CurrentStamina; // Automatically set to _Stamina on start
        public float CurrentMana; // Automatically set to _Mana on start

        public IStatusEffect[] StatusEffects; // Current status effects on entity
        
        public float MaxHealth => BaseHealth; //todo figure out scaling
        public float MaxStamina => BaseStamina; //todo figure out scaling
        public float MaxMana => BaseMana; //todo figure out scaling
        public float Strength => BaseStrength; //todo figure out scaling
        public float Speed => BaseSpeed; //todo figure out scaling
        public float Defense => BaseDefense; //todo figure out scaling
        
        void Start()
        {
            CurrentHealth = MaxHealth;
            CurrentStamina = MaxStamina;
            CurrentMana = MaxMana;
        }
        public void Damage(float amt)
        {
            CurrentHealth -= amt;
        }
        // todo include methods to get the actual values that includes effects from equipment, buffs, etc.
    }
}