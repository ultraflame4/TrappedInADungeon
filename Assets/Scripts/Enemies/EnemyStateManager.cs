using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyStateManager : MonoBehaviour
    {
        public EnemyStateBehaviour Idle;
        public EnemyStateBehaviour Alert;
        public EnemyStateBehaviour Attack;
        public EnemyStateBehaviour Stunned;
        private EnemyStateBehaviour currentState;
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
            if (currentState is not null)
            {
                currentState.stateActive = false;
                currentState.StateExit();
            }
            currentState = GetStateBehavior(state);
            if (currentState is null) throw new NullReferenceException($"State {state} does not exist");
            currentState.stateActive = true;
            currentState.StateEnter();
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

        }
    }
}