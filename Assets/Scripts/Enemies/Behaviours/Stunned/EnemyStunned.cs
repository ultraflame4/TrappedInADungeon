using System.Collections;
using Core.Enemies;
using Enemies.Behaviours.Follow;
using UnityEngine;

namespace Enemies.Behaviours.Stunned
{
    [RequireComponent(typeof(EnemyFollow))]
    public class EnemyStunned : EnemyStateBehaviour
    {
        // cOmpOnenT rEfErEnCeS
        private EnemyFollow enemyFollow;
        [Tooltip("Amount of time to stay stunned.")]
        public float stunPeriodSecs = 0.5f;

        private void Start()
        {
            // Get component references
            enemyFollow = GetComponent<EnemyFollow>();
        }

        public override void StateEnter()
        {
            // When enter stun,
            // Make the enemy fly backwards away from the player
            Vector2 knockbackForce = ((Vector2)(-enemyFollow.directionToPlayerSnapped) + Vector2.up*2).normalized*Random.Range(3,5);
            stateManager.rb.velocity=knockbackForce;
            // Start stun end coroutine
            StartCoroutine(StunEnd());
        }
        
        IEnumerator StunEnd()
        {
            // Wait for stun period before transitioning back to alert state
            yield return new WaitForSeconds(stunPeriodSecs);
            stateManager.TransitionState(EnemyStates.ALERT);
        }
    }
}