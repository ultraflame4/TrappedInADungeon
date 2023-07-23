using Core.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level
{
    [RequireComponent(typeof(ParticleSystem)), RequireComponent(typeof(InteractableObject))]
    public class PortalInteraction : MonoBehaviour
    {
        public float activeRadialMulti = 1f;
        public float activeEmissionRate = 15f;

        /// <summary>
        /// Whether this portal is at the start or end of the level.
        /// </summary>
        public bool IsStartPortal = false;

        public string StartPortalText = "Return to previous area";
        public string EndPortalText = "Go to next area";
        public InteractableObject interactableObject { get; private set; }
        private float initialRadialMulti;
        private float initialEmissionRate;
        private ParticleSystem particleSys;

        private void Start()
        {
            particleSys = GetComponent<ParticleSystem>();
            interactableObject = GetComponent<InteractableObject>();
            interactableObject.popupText = IsStartPortal ? StartPortalText : EndPortalText;
            interactableObject.InteractableChange += OnInteractableChange;
            interactableObject.InteractedWith += OnInteractedWith;
            initialRadialMulti = particleSys.velocityOverLifetime.radialMultiplier;
            initialEmissionRate = particleSys.emission.rateOverTimeMultiplier;
        }

        void OnInteractableChange(bool value)
        {
            ParticleSystem.VelocityOverLifetimeModule velOverLifetime = particleSys.velocityOverLifetime;
            ParticleSystem.EmissionModule emissionModule = particleSys.emission;
            velOverLifetime.radialMultiplier = value ? activeRadialMulti : initialRadialMulti;
            emissionModule.rateOverTimeMultiplier = value ? activeEmissionRate : initialEmissionRate;
        }

        void OnInteractedWith()
        {
            if (!IsStartPortal)
            {
                GameManager.Instance.LoadNextArea();
                return;
            }

            if (GameManager.CurrentAreaIndex == 0)
            {
                NotificationManager.Instance.PushNotification("<color=\"red\">In 1st area! No more previous area!</color>");
                return;
            }

            GameManager.Instance.LoadPrevArea();
        }
    }
}