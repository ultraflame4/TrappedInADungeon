using Core.UI;
using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(Movement)),RequireComponent(typeof(SpriteRenderer)),RequireComponent(typeof(PlayerBody))]
    public class PlayerDeathEffect : MonoBehaviour
    {
        public GameObject DeadShadowPrefab;
        private PlayerBody body;
        private SpriteRenderer spriteRenderer;
        private Movement movement;
        private void Start()
        {
            movement = GetComponent<Movement>();
            body = GetComponent<PlayerBody>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            body.DeathEvent += OnDeath;
        }

        private void OnDeath()
        {
            var shadow = Instantiate(DeadShadowPrefab, transform.position, transform.rotation);
            shadow.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite; // Set shadow sprite to the sprite of this entity
            NotificationManager.Instance.PushNotification("<color=#f00><size=150%>You died!</size></color>");
            movement.enabled = false;
            spriteRenderer.enabled = false;
            
        }
    }
}