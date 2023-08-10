using System;
using Core.Entities;
using Core.Sound;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities
{
    /// <summary>
    /// A bunch of common effects for entity body
    /// </summary>
    [RequireComponent(typeof(EntityBody))]
    public class EntityDamagedEffects : MonoBehaviour
    {
        public GameObject bloodParticles;
        [FormerlySerializedAs("damageNumbers")]
        public GameObject dmgNumberPrefab;

        [SerializeField]
        private SoundEffect soundEffect;

        private EntityBody entityBody;
        private static int dmgNumberCount = 0; // How many instances of the damage numbers are there in the world
        private const int maxDmgNumberCount = 100;

        private void Awake()
        {
            soundEffect.Create(gameObject);
            dmgNumberCount = 0;
            entityBody = GetComponent<EntityBody>();
            entityBody.DamagedEvent += OnDamaged;
        }


        void SpawnDamageNumbers(float amt)
        {
            // If no damage number prefab specified skip
            if (dmgNumberPrefab == null) return;
            // If there too many dmg numbers, it will slow down the game, hence if above threshold skip
            if (dmgNumberCount >= maxDmgNumberCount) return;
            // We are spawning new dmgNumber therefore increase count
            dmgNumberCount++;
            // Instantiate the dmgNumbers and configure it accordingly
            var damageNumber = Instantiate(dmgNumberPrefab, transform.position, Quaternion.identity).GetComponent<EntityDamageNumber>();
            damageNumber.number = amt;
            damageNumber.targetMaxHealth = entityBody.Health;
            // When dmgNumber is destroyed, reduce the dmgNumberCount so that we can spawn more dmgNumber if needed.
            damageNumber.destroyCallback = () => { dmgNumberCount--; };
        }

        void OnDamaged(float amt, bool stun)
        {
            // When damaged, create the blood particles by spawning the bloodParticle prefab
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
            // Show dmgNumbers
            SpawnDamageNumbers(amt);
            // Check if audioSource component is enabled, this fixes error: Cannot Play Audio when component is disabled warnings 
            // (prob due to it trying to play when the entity died & is being destroyed)
            if (soundEffect.audioSrc.isActiveAndEnabled) soundEffect.audioSrc.Play();
        }
    }
}