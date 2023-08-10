using System.Collections;
using PlayerScripts;
using UnityEngine;

namespace Loot
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ExpBall : MonoBehaviour
    {
        public float expValue;
        public float moveDelaySecs = 0.3f;
        private bool move = false;

        private void Start()
        {
            // Only start moving to the player after delay.
            StartCoroutine(StartMoveToPlayer());
        }

        IEnumerator StartMoveToPlayer()
        {
            // Only start moving to the player after delay.
            yield return new WaitForSeconds(moveDelaySecs);
            move = true;
        }
        private void FixedUpdate()
        {
            // if waiting for delay, return
            if (!move) return;
            // Linearly interpolate to the player position
            transform.position = Vector3.Lerp(transform.position,Player.Transform.position , 0.1f);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            // If not player, skip
            if (!other.CompareTag("Player")) return;
            // When player enters the collider, player absorbs the exp. So add to the player
            Player.Body.AddExperiencePoints(expValue);
            // Destroy gameObject after absorbed by player
            Destroy(gameObject);
        }
    }
}