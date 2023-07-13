﻿using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyIdle : EnemyStateBehaviour
    {
        public float eyeSightRange = 2f;
        public float eyeSightOffset = 0.5f;
        Vector3 raycastOrigin => transform.position + Vector3.up * eyeSightOffset;
        
        
        private bool CheckPlayerVisible()
        {
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, transform.right, eyeSightRange, LayerMask.GetMask("Player"));
            if (!hit.transform) return false;
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
            return false;
        }

        private void FixedUpdate()
        {
            if (!stateActive) return;
            if (CheckPlayerVisible())
            {
                stateManager.TransitionState(EnemyStates.ALERT);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(raycastOrigin, transform.right * eyeSightRange);
        }
    }
}