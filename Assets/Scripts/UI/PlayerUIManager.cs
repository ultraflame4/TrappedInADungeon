using Core.UI;
using Entities;
using TMPro;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerUIManager : MonoBehaviour
    {
        public PlayerBody playerBody;
        public BarController healthBar;
        public BarController manaBar;
        public TextMeshProUGUI levelText;
        private void Start()
        {
            UpdateHealthBar();
            UpdateManaBar();
            UpdatePlayerLevel(playerBody.Level);
            playerBody.CurrentHealth.Changed += () =>
            {
                UpdateHealthBar();
            };
            playerBody.CurrentMana.Changed += () =>
            {
                UpdateManaBar();
            };
            playerBody.PlayerLevelChanged += (newLevel) =>
            {
                UpdatePlayerLevel(newLevel);
                UpdateHealthBar();
                UpdateManaBar();
            };
        }

        void UpdateHealthBar()
        {
            healthBar.filledPercentage = playerBody.CurrentHealth.value / playerBody.Health;
            healthBar.UpdateBar();
        }
        void UpdateManaBar()
        {
            manaBar.filledPercentage = playerBody.CurrentMana.value / playerBody.Mana;
            manaBar.UpdateBar();
        }

        void UpdatePlayerLevel(int newLevel)
        {
            levelText.text = $"{newLevel}";
        }
    }
}