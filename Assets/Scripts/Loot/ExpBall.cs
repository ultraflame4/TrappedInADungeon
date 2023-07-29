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
            StartCoroutine(MoveToPlayer());
        }

        IEnumerator MoveToPlayer()
        {
            yield return new WaitForSeconds(moveDelaySecs);
            move = true;
        }
        private void FixedUpdate()
        {
            if (!move) return;
            transform.position = Vector3.Lerp(transform.position,Player.Transform.position , 0.1f);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            Player.Body.AddExperiencePoints(expValue);
            Destroy(gameObject);
        }
    }
}