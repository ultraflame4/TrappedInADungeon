using Entities;
using UI;
using UnityEngine;

namespace Player
{
    public class BarManager : MonoBehaviour
    {
        public EntityBody playerBody;
        public BarController healthBar;
        
        private void Start()
        {
            playerBody.OnDamagedEvent += () =>
            {
                healthBar.filledPercentage = playerBody.CurrentHealth / playerBody.Health;
                healthBar.UpdateBar();
            };
        }
    }
}