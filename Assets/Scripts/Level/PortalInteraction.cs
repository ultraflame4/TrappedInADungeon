using Core.UI;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(ParticleSystem)), RequireComponent(typeof(InteractableObject))]
    public class PortalInteraction : MonoBehaviour
    {
        [Tooltip("Active Particle system parameter: Velocity radial multiplier")]
        public float activeRadialMulti = 1f;
        [Tooltip("Active Particle system parameter: Particle emission rate")]
        public float activeEmissionRate = 15f;

        [Tooltip("Whether this portal is at the start or end of the level.")]
        public bool IsStartPortal = false;

        [Tooltip("Text to use if is Start Portal")]
        public string StartPortalText = "Return to previous area";
        [Tooltip("Text to use if is End Portal")]
        public string EndPortalText = "Go to next area";
        public InteractableObject interactableObject { get; private set; }
        
        private float initialRadialMulti; // Initial parameter set in the particle system
        private float initialEmissionRate; // Initial parameter set in the particle system
        
        private ParticleSystem particleSys; // component reference
        private bool alreadyLoading = false; // if the portal is already loading next level.

        private void Start()
        {
            // Get components
            particleSys = GetComponent<ParticleSystem>();
            interactableObject = GetComponent<InteractableObject>();
            // Set the interact popup text 
            interactableObject.popupText = IsStartPortal ? StartPortalText : EndPortalText;
            // Register event for when this object becomes interactable with player
            interactableObject.InteractableChange += OnInteractableChange;
            // Register event for when player interacts with this object
            interactableObject.InteractedWith += OnInteractedWith;
            // Remember the initial particle system parameters
            initialRadialMulti = particleSys.velocityOverLifetime.radialMultiplier;
            initialEmissionRate = particleSys.emission.rateOverTimeMultiplier;
        }

        int GetEnemiesCount() => GameObject.FindGameObjectsWithTag("Enemy").Length; // Shorthand to get number of enemies remaining

        void OnInteractableChange(bool value)
        {
            // When player is close enough to interact with this object,
            // change the particle system parameters to have a slightly different particle effect 
            // vice versa
            ParticleSystem.VelocityOverLifetimeModule velOverLifetime = particleSys.velocityOverLifetime; // Get the vel module
            ParticleSystem.EmissionModule emissionModule = particleSys.emission; // Get the emission module
            velOverLifetime.radialMultiplier = value ? activeRadialMulti : initialRadialMulti; // Update vel param
            emissionModule.rateOverTimeMultiplier = value ? activeEmissionRate : initialEmissionRate; // Update emission param
        }

        void OnInteractedWith()
        {
            // If already loading new level, push notification and skip
            if (alreadyLoading)
            {
                NotificationManager.Instance.PushNotification("<color=#f5c400>Already loading! Please Wait!</color>");
                return;
            }
            // If this isn't the start portal. this is the end portal
            if (!IsStartPortal)
            {
                // If this is the end portal, if there are enemies left, do nothing (dont let player go to next area)
                int enemiesLeft = GetEnemiesCount();
                if ( enemiesLeft> 0)
                {
                    NotificationManager.Instance.PushNotification($"<size=120%>All enemies must be Killed!</size>");
                    NotificationManager.Instance.PushNotification($"<color=\"red\">Enemies left: ",addData:$"{enemiesLeft}</color>");
                    return;
                }
                
                // Else set already loading to true
                alreadyLoading = true;
                // And also push a notification informing the player
                NotificationManager.Instance.PushNotification($"Moving to next area - Area {GameManager.CurrentAreaIndex+1}");
                // Tell GameManager to load next area
                GameManager.Instance.LoadNextArea();
                return;
            }
            // If index of current area is 0, player alr is in first / spawn area. There is no more prev areas
            if (GameManager.CurrentAreaIndex == 0)
            {
                // Push notification to inform player
                NotificationManager.Instance.PushNotification("<color=\"red\">In 1st area! No more previous area!</color>");
                return;
            }
            // Prevent duplicate loading
            alreadyLoading = true;
            // Inform player we are loading prev area
            NotificationManager.Instance.PushNotification($"Moving to previous area - Area {GameManager.CurrentAreaIndex-1}");
            // Tell GameManager to load prev area
            GameManager.Instance.LoadPrevArea();
        }
    }
}