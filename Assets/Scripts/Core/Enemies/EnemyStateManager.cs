using System;
using System.Collections;
using Core.Entities;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Enemies
{
    [RequireComponent(typeof(SpriteRenderer)),RequireComponent(typeof(EntityBody)), RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator))]
    public class EnemyStateManager : MonoBehaviour
    {
        public Animator animator { get; private set; }
        public Rigidbody2D rb { get; private set; }
        public EntityBody body { get; private set; }
        private SpriteRenderer spriteRenderer;
        

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
            spriteRenderer = GetComponent<SpriteRenderer>();
            body = GetComponent<EntityBody>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            Idle?.StateInit(this);
            Alert?.StateInit(this);
            Attack?.StateInit(this);
            Stunned?.StateInit(this);

            body.DamagedEvent += OnDamaged;
            body.DeathEvent += OnDeath;
            TransitionState(EnemyStates.PATROL);
        }

        void OnDamaged(float amt, bool stun)
        {
            if (Stunned != null && stun)
            {
                TransitionState(EnemyStates.STUNNED);
            }
        }

        void OnDeath()
        {
            TransitionState(EnemyStates.DEAD);
            rb.velocity= Vector2.zero;
            spriteRenderer.enabled = false;
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
            
            if (currentState == EnemyStates.DEAD) return; // Dead no need to enter another state

            if (currentBehaviour is null)
            {
                Debug.LogWarning($"State {state} does not exist");
                return;
            }

            ;
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
                case EnemyStates.DEAD:
                    return null;
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