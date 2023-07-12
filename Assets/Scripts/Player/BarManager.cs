using Entities;
using UI;
using UnityEngine;

namespace Player
{
    public class BarManager : MonoBehaviour
    {
        public PlayerBody playerBody;
        public BarController healthBar;
        public BarController manaBar;
        
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
        }
    }
}