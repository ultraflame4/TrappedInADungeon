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
        [field: SerializeField]
        public EnemyStates currentState { get; private set; }
        private EnemyStateBehaviour currentBehaviour;
        public bool isGrounded { get; private set; } = false;

        private void Start()
        {
            Idle?.StateInit(this);
            Alert?.StateInit(this);
            Attack?.StateInit(this);
            Stunned?.StateInit(this);
            TransitionState(EnemyStates.PATROL);
        }

        /// <summary>
        /// Call this to transition to a new state.
        /// If the state is the same as the current state, nothing will happen.
        /// </summary>
        /// <param name="state">The state to transition to</param>
        public void TransitionState(EnemyStates state)
        {
            // Debug.Log($"Transitioning to {state}");
            if (currentState == state) return;

            if (currentBehaviour is not null)
            {
                currentBehaviour.stateActive = false;
                currentBehaviour.StateExit();
            }

            currentBehaviour = GetStateBehavior(state);
            currentState = state;
            if (state == EnemyStates.STUNNED)
            {
                animator.SetTrigger("Stun");
            }

            if (currentBehaviour is null)
            {
                Debug.LogWarning($"State {state} does not exist");
                return;
            };
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
        }

        private void Update()
        {
            animator.SetBool("isWalking", rb.velocity.magnitude > 0);
        }

        public void SetAttackAnim(bool value)
        {
            animator.SetBool("Attack", value);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
    }
}