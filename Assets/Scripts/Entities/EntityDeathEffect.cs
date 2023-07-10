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
            entityBody.DeathEvent += OnDeath;
        }

        void OnDeath()
        {
            var shadow = Instantiate(DeadShadowPrefab, transform.position, transform.rotation);
            shadow.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite; // Set shadow sprite to the sprite of this entity
            Destroy(gameObject);
        }

    }
}
// todo move this file to another location