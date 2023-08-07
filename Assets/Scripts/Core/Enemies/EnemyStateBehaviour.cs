using Enemies;
using UnityEngine;

namespace Core.Enemies
{
    public abstract class EnemyStateBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Whether the state is active or not. Use this to determine if the Update & other code should be ran.
        /// </summary>
        public bool stateActive { get; set; } = false;

        /// <summary>
        /// The state manager that this state is attached to
        /// </summary>
        public EnemyStateManager stateManager { get; private set; }

        public void StateInit(EnemyStateManager manager)
        {
            stateManager = manager;
        }

        /// <summary>
        /// Called when the state is entered
        /// </summary>
        public virtual void StateEnter() { }

        /// <summary>
        /// Called when state exits
        /// </summary>
        public virtual void StateExit() { }
    }
}