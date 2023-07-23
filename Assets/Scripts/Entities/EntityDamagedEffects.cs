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
        private EntityBody entityBody;

        private void Start()
        {
            entityBody = GetComponent<EntityBody>();
            entityBody.DamagedEvent+= OnDamaged;
        }

        void OnDamaged()
        {
            Instantiate(bloodParticles, transform.position, Quaternion.identity);
        }
    }
}
