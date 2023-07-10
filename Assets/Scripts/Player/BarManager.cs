using Entities;
using UI;
using UnityEngine;

namespace Player
{
    public class BarManager : MonoBehaviour
    {
        public EntityBody playerBody;
        public BarController healthBar;
        public BarController manaBar;
        
        private void Start()
        {
            playerBody.HealthChangedEvent += () =>
            {
                healthBar.filledPercentage = playerBody.CurrentHealth / playerBody.Health;
                healthBar.UpdateBar();
            };
            playerBody.ManaChangedEvent += () =>
            {
                manaBar.filledPercentage = playerBody.CurrentMana / playerBody.Mana;
                manaBar.UpdateBar();
            };
        }
    }
}