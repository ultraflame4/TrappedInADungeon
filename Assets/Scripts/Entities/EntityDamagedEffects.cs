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
            entityBody.DamagedEvent+= OnDamaged;
        }

        void OnDamaged()
        {
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
        }
    }
}
// todo move this file to another location