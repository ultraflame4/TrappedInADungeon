using System;
using System.Collections;
using Core.Entities;
using Core.Sound;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Adds a death effect when the entity dies. This also destroys the entity.
    /// </summary>
    [RequireComponent(typeof(EntityBody), typeof(SpriteRenderer))]
    public class EntityDeathEffect : MonoBehaviour
    {
        public GameObject DeadShadowPrefab;
        [SerializeField]
        private SoundEffect DeathSoundEffect;
        private EntityBody entityBody;
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            entityBody = GetComponent<EntityBody>();
            entityBody.DeathEvent += OnDeath;
        }

        void OnDeath()
        {
            var shadow = Instantiate(DeadShadowPrefab, transform.position, transform.rotation);
            shadow.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite; // Set shadow sprite to the sprite of this entity
            DeathSoundEffect.Create(shadow); // Add the audio source to the shadow
            DeathSoundEffect.audioSrc.Play(); // play it
            Destroy(gameObject);
            
        }



    }
}