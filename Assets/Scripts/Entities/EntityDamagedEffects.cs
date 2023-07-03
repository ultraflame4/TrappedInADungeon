using System;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// A bunch of common effects for entity body
    /// </summary>
    [RequireComponent(typeof(EntityBody))]
    public class EntityDamagedEffects : MonoBehaviour
    {
        public EntityBody entityBody;
        public GameObject bloodParticles;

        private void Start()
        {
            entityBody.OnDamagedEvent+= OnDamaged;
        }

        void OnDamaged()
        {
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
            // todo move the particles to the weapon for performance reasons.
        }
    }
}