using Core.UI;
using PlayerScripts;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Script that manages the player UI
    /// </summary>
    public class PlayerUIManager : MonoBehaviour
    {
        // Component references to the various ui elements
        public GameObject inventoryUi;
        public BarController healthBar;
        public BarController manaBar;
        public TextMeshProUGUI levelText;

        private void Start()
        {
            // Hide the inventory ui on start
            inventoryUi.SetActive(false);
            // Add a listener to the inventory toggle keybind
            GameManager.Controls.Menus.InventoryToggle.performed += (ctx) => inventoryUi.SetActive(!inventoryUi.activeSelf);
            // Update the health and mana bars so they show the correct values
            UpdateHealthBar();
            UpdateManaBar();
            // Update the player level text so they show the correct values
            UpdatePlayerLevel(Player.Body.Level);
            // Add listeners to the health and mana values so the bars update when they change
            Player.Body.CurrentHealth.Changed += UpdateHealthBar;
            Player.Body.CurrentMana.Changed += UpdateManaBar;
            // Add a listener to the player level so the text updates when it changes
            Player.Body.PlayerLevelChanged += (newLevel) =>
            {
                UpdatePlayerLevel(newLevel);
                UpdateHealthBar();
                UpdateManaBar();
            };
        }

        void UpdateHealthBar()
        {
            // If player is dead, set health bar to 0
            if (Player.Body.IsDead)
            {
                healthBar.filledPercentage = 0;
                healthBar.UpdateBar();
                return;
            }
            // Otherwise, set the health bar to the correct value :D
            healthBar.filledPercentage = Player.Body.CurrentHealth.value / Player.Body.Health;
            healthBar.UpdateBar();
        }

        void UpdateManaBar()
        {
            // Update mana bar to the correct value :D
            manaBar.filledPercentage = Player.Body.CurrentMana.value / Player.Body.Mana;
            manaBar.UpdateBar();
        }

        void UpdatePlayerLevel(int newLevel)
        {
            // Update the player level text to the correct value :D
            levelText.text = $"{newLevel}";
        }
    }
}