using System;
using Core.Entities;
using Core.Sound;
using PlayerScripts;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// A bunch of common effects for entity body
    /// </summary>
    [RequireComponent(typeof(EntityBody))]
    public class EntityDamagedEffects : MonoBehaviour
    {
        public GameObject bloodParticles;
        public GameObject damageNumbers;

        [SerializeField]
        private SoundEffect soundEffect;

        private EntityBody entityBody;
        private static int dmgNumberCount = 0;
        private const int maxDmgNumberCount = 100;

        private void Awake()
        {
            soundEffect.Create(gameObject);

            entityBody = GetComponent<EntityBody>();
            entityBody.DamagedEvent += OnDamaged;
        }


        void SpawnDamageNumbers(float amt)
        {
            if (damageNumbers == null) return;
            if (dmgNumberCount >= maxDmgNumberCount) return;
            dmgNumberCount++;
            var damageNumber = Instantiate(damageNumbers, transform.position, Quaternion.identity).GetComponent<EntityDamageNumber>();
            damageNumber.number = amt;
            damageNumber.targetMaxHealth = entityBody.Health;
            damageNumber.destroyCallback = () => { dmgNumberCount--; };
        }

        void OnDamaged(float amt, bool stun)
        {
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
            SpawnDamageNumbers(amt);
            // Check if audioSource component is enabled -> fixes Cannot Play Audio when component is disabled warnings 
            // (prob due to it trying to play when the entity died & is being destroyed)
            if (soundEffect.audioSrc.isActiveAndEnabled) soundEffect.audioSrc.Play();
            
        }
    }
}