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
            playerBody.CurrentHealth.Changed += () =>
            {
                healthBar.filledPercentage = playerBody.CurrentHealth.value / playerBody.Health;
                healthBar.UpdateBar();
            };
            playerBody.CurrentMana.Changed += () =>
            {
                manaBar.filledPercentage = playerBody.CurrentMana.value / playerBody.Mana;
                manaBar.UpdateBar();
            };
            playerBody.PlayerLevelChanged += (newLevel) =>
            {
                levelText.text = $"{newLevel}";
            };
        }
    }
}