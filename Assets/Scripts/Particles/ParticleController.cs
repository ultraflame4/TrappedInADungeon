using UnityEngine;

namespace Particles
{
    public class ParticleController : MonoBehaviour
    {
        public ParticleSystem ps;

        private void Start()
        {
            ps = GetComponent<ParticleSystem>();
            if (ps is null)
            {
                Debug.LogError("Particle system not found on " + gameObject.name);
                return;
            }

            var main = ps.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
        }
    }
}