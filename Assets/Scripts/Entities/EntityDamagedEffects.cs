using System;
using Core.Entities;
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
        private EntityBody entityBody;
        

        private void Start()
        {
            entityBody = GetComponent<EntityBody>();
            entityBody.DamagedEvent+= OnDamaged;
        }

        void OnDamaged(float amt)
        {
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
            if (damageNumbers == null) return;
            var damageNumber = Instantiate(damageNumbers, transform.position, Quaternion.identity).GetComponent<EntityDamageNumber>();
            damageNumber.number = amt;
            damageNumber.targetMaxHealth = entityBody.Health;
        }
    }
}
