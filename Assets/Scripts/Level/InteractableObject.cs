using System;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractableObject : MonoBehaviour
    {

        [Tooltip("The text to show when the player is in the interactable zone. ( When the player is inside the boxCollider trigger)"),Multiline]
        public string popupText = "No text set for this interaction!";
        
        private BoxCollider2D boxCollider2D;
        /// <summary>
        /// Invoked when player enters or leave the interactable zone;
        /// value is true when player enters the interactable zone, false when  player exits.
        /// </summary>
        public event Action<bool> InteractableZoneEnter; 
        public bool interactableZoneActive { get; private set; }
        private void Start()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            interactableZoneActive = true;
            InteractableZoneEnter?.Invoke(true);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            interactableZoneActive = false;
            InteractableZoneEnter?.Invoke(false);
        }
    }
}