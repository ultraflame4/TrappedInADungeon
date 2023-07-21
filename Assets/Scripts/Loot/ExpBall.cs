﻿using System.Collections;
using Player;
using UnityEngine;

namespace Loot
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class ExpBall : MonoBehaviour
    {
        public float expValue;
        public float moveDelaySecs = 0.3f;
        private bool move = false;
        private PlayerBody player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerBody>();
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
            transform.position = Vector3.Lerp(transform.position,player.transform.position , 0.1f);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            player.AddExperiencePoints(expValue);
            Destroy(gameObject);
        }
    }
}