using System;
using Entities;
using UnityEngine;

namespace Enemies
{
    public class EnemyController : MonoBehaviour
    {
        public Rigidbody2D rb;
        public EntityBody body;
        public Transform player;
        private Vector3 directionToPlayer;
        
        public float knockbackForce = 100f;
        public EnemyState state;
        // todo implement state machine ai stuff

        private void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
            body.OnDamagedEvent += OnAttacked;
        }

        public void OnAttacked()
        {
            rb.velocity = (Vector3.up - directionToPlayer/4).normalized * knockbackForce;
        }

        private void Update()
        {
            directionToPlayer = (player.position - transform.position).normalized;

            switch (state)
            {
                case EnemyState.STUNNED:
                    break;
                case EnemyState.PATROL:
                    break;
                case EnemyState.ATTACK:
                    break;
                case EnemyState.MOVE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}