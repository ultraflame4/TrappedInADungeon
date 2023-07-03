using System;
using System.Collections;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Adds a death effect when the entity dies. This also destroys the entity.
    /// </summary>
    [RequireComponent(typeof(EntityBody), typeof(SpriteRenderer))]
    public class EntityDeathEffect : MonoBehaviour
    {
        public EntityBody entityBody;
        public SpriteRenderer spriteRenderer;
        public GameObject DeadShadowPrefab;

        private void Start()
        {
            entityBody.OnDeathEvent += OnDeath;
        }

        void OnDeath()
        {
            Destroy(gameObject);
        }

    }
}