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

        public float Mana => BaseMana * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.Mana);
        public float ManaRegen => BaseManaRegen * Level + StatsModifiers.Sum(modifier => modifier.statsModifier.ManaRegen);
        [field: SerializeField] [JsonProperty]
        public VolatileValue<float> CurrentMana { get; private set; }  = new(); // Automatically set to Mana on start
        [JsonProperty]
        public float currentExp { get; private set; } = 0f;
        public float NextLevelExp => Mathf.Pow(Level*0.65f,2);
        public event Action<int> PlayerLevelChanged;

        protected override void Awake()
        {
            base.Awake();

            CurrentMana.validator = (value, newValue) => Mathf.Min(Mana, newValue);
            GameSaveManager.AddSaveHandler("player.body",this);
        }

        protected override void Start()
        {
            base.Start();
            CurrentMana.value = Mana;
        }
        
        private void Update()
        {
            CurrentMana.value = Mathf.Min(CurrentMana.value + ManaRegen * Time.deltaTime, Mana);
        }

        public void AddExperiencePoints(float amt)
        {
            currentExp += amt;
            if (currentExp > NextLevelExp)
            {
                Level++;
                CurrentHealth.value = Health;
                NotificationManager.Instance.PushNotification($"<size=150%>Leveled Up - Lv {Level}!</size>");
                NotificationManager.Instance.PushNotification($"<color=\"red\">Health restored</color>");
                PlayerLevelChanged?.Invoke(Level);
                currentExp = 0;
            }
        }
        
    }
}