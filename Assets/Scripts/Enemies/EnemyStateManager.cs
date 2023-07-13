using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyStateManager : MonoBehaviour
    {
        public Animator animator;
        public Rigidbody2D rb;

        public EnemyStateBehaviour Idle;
        public EnemyStateBehaviour Alert;
        public EnemyStateBehaviour Attack;
        public EnemyStateBehaviour Stunned;
        private EnemyStateBehaviour currentBehaviour;
        private EnemyStates currentState2;

        private void Start()
        {
            Idle?.StateInit(this);
            Alert?.StateInit(this);
            Attack?.StateInit(this);
            Stunned?.StateInit(this);
            TransitionState(EnemyStates.PATROL);
        }

        public void TransitionState(EnemyStates state)
        {
            if (currentBehaviour is not null)
            {
                currentBehaviour.stateActive = false;
                currentBehaviour.StateExit();
            }
            currentBehaviour = GetStateBehavior(state);
            currentState2 = state;
            if (state == EnemyStates.STUNNED)
            {
                animator.SetTrigger("Stun");
            }
            
            if (currentBehaviour is null) throw new NullReferenceException($"State {state} does not exist");
            currentBehaviour.stateActive = true;
            currentBehaviour.StateEnter();
        }
        public EnemyStateBehaviour GetStateBehavior(EnemyStates state)
        {
            switch (state)
            {
                case EnemyStates.STUNNED:
                    return Stunned;
                case EnemyStates.PATROL:
                    return Idle;
                case EnemyStates.ALERT:
                    return Alert;
                case EnemyStates.ATTACK:
                    return Attack;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            return null;
        }

        private void Update()
        {
            animator.SetBool("isWalking", rb.velocity.magnitude > 0);
            animator.SetBool("Attack", currentState2 == EnemyStates.ATTACK);
        }
    }
}