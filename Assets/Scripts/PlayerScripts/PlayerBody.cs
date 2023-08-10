using System;
using System.Linq;
using Core.Entities;
using Core.Save;
using Core.UI;
using Core.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace PlayerScripts
{
    [Serializable]
    public class PlayerBody : EntityBody, IMagicStats, ISaveHandler
    {
        [Header("Mana")]
        public int BaseMana; // Used for casting spells

        [Tooltip("Rate of mana regeneration per second")]
        public int BaseManaRegen; // Used for casting spells

        /// <summary>
        /// Total mana of the player
        /// </summary>
        public float Mana => BaseMana * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.Mana);

        /// <summary>
        /// Mana regeneration of the player
        /// </summary>
        public float ManaRegen => BaseManaRegen * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.ManaRegen);

        /// <summary>
        /// Amount of Mana the player currently has 
        /// </summary>
        [field: SerializeField]
        [JsonProperty]
        public VolatileValue<float> CurrentMana { get; private set; } = new(); // Automatically set to Mana on start

        /// <summary>
        /// Amount of exp the player currently has.
        /// </summary>
        [JsonProperty]
        public float currentExp { get; private set; } = 0f;

        /// <summary>
        /// Amount of exp the player needs to advance to the next level.
        /// </summary>
        public float NextLevelExp => Mathf.Pow(Level * 0.65f, 2); // equation for exp
        /// <summary>
        /// Event invoked when player level is increased or reduced
        /// </summary>
        public event Action<int> PlayerLevelChanged;

        protected override void Awake()
        {
            base.Awake(); // Call the base method
            // Add the validator for CurrentMana to ensure it doesnt go above max mana
            CurrentMana.validator = (value, newValue) => Mathf.Min(Mana, newValue);
            // Register this object to be saved
            GameSaveManager.AddSaveHandler("player.body", this);
        }

        protected override void Start()
        {
            base.Start(); // Call the base method
            // Set the current mana to max mana aka player starts off with max mana
            CurrentMana.value = Mana;
        }

        private void Update()
        {
            // Mana regeneration over time
            CurrentMana.value = Mathf.Min(CurrentMana.value + ManaRegen * Time.deltaTime, Mana);
        }

        /// <summary>
        /// Adds experience points to the player
        /// </summary>
        /// <param name="amt"></param>
        public void AddExperiencePoints(float amt)
        {
            // Add to the currentExp
            currentExp += amt;
            // If player exceeds or has enough exp for the next level,
            if (currentExp >= NextLevelExp)
            {
                Level++; // Increase player level
                CurrentHealth.value = Health; // Restore player health
                // Push some helpful notifications
                NotificationManager.Instance.PushNotification($"<size=150%>Leveled Up - Lv {Level}!</size>");
                NotificationManager.Instance.PushNotification($"<color=\"red\">Health restored</color>");
                // invoke player level changed event
                PlayerLevelChanged?.Invoke(Level);
                // Reset current exp
                currentExp = 0;
            }
        }
    }
}