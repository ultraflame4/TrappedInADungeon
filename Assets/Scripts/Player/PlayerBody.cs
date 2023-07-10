using System;
using System.Collections;
using System.Linq;
using Entities;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerBody : EntityBody, IMagicStats
    {
        [Header("Mana")]
        public int BaseMana; // Used for casting spells
        [Tooltip("Rate of mana regeneration per second")]
        public int BaseManaRegen; // Used for casting spells
        public float Mana => BaseMana * Level + StatsModifiers.Sum(modifier => modifier.Mana);
        public float ManaRegen => BaseManaRegen * Level + StatsModifiers.Sum(modifier => modifier.ManaRegen);
        public VolatileValue<float> CurrentMana = new(); // Automatically set to Mana on start

        protected override void Start()
        {
            base.Start();
            CurrentMana.value = Mana;
        }

        private void Update()
        {
            CurrentMana.value = Mathf.Min(CurrentMana.value + ManaRegen * Time.deltaTime, Mana);
        }
    }
}