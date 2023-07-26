using System.Collections;
using Core.Enemies;
using Enemies.Behaviours.Follow;
using UnityEngine;

namespace Enemies.Behaviours.Stunned
{
    [RequireComponent(typeof(EnemyFollow))]
    public class EnemyStunned : EnemyStateBehaviour
    {
        private EnemyFollow enemyFollow;
        [Tooltip("Amount of time to stay stunned.")]
        public float stunPeriodSecs = 0.5f;

        private void Start()
        {
            enemyFollow = GetComponent<EnemyFollow>();
        }

        public override void StateEnter()
        {
            Vector2 knockbackForce = ((Vector2)(-enemyFollow.directionToPlayerSnapped) + Vector2.up*2).normalized*Random.Range(3,5);
            stateManager.rb.velocity=knockbackForce;
            
            StartCoroutine(StunEnd());
        }
        
        IEnumerator StunEnd()
        {
            yield return new WaitForSeconds(stunPeriodSecs);
            stateManager.TransitionState(EnemyStates.ALERT);
        }
    }
}