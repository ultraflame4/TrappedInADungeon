using System;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyStateBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Whether the state is active or not. Use this to determine if the Update & other code should be ran.
        /// </summary>
        public bool stateActive { get; set; } = false;
        private EnemyStateManager _stateManager;
        public EnemyStateManager stateManager => _stateManager;
        public void StateInit(EnemyStateManager manager)
        {
            _stateManager = manager;
        }

        /// <summary>
        /// Called when the state is entered
        /// </summary>
        public void StateEnter() { }

        /// <summary>
        /// Called when state exitss
        /// </summary>
        public void StateExit(){}
        
    }
}