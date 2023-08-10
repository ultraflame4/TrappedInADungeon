using System;
using Core.UI.InteractText;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Level
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class InteractableObject : MonoBehaviour
    {

        [Tooltip("The text to show when the player is in the interactable zone. ( When the player is inside the boxCollider trigger)"),Multiline]
        public string popupText = "No text set for this interaction!";
        [Tooltip("Offset of the text position;")]
        public Vector2 offset;

        /// <summary>
        /// Invoked when player enters or leave the interactable zone;
        /// value is true when player enters the interactable zone, false when  player exits.
        /// </summary>
        public event Action<bool> InteractableChange;
        /// <summary>
        /// Invoked when the player has interacted with this object.
        /// </summary>
        public event Action InteractedWith; 
        /// <summary>
        /// When true, player can interact with this object
        /// </summary>
        public bool interactableZoneActive { get; private set; }

        /// <summary>
        /// Use this to push interaction text popups
        /// </summary>
        private InteractTextHandler interactText;
        private void Awake()
        {
            // Create a new interactTextHandler to easily push text
            interactText = InteractTextManager.Instance.Create();
            // Register callback for player interact input action
            GameManager.Controls.Player.Interact.performed += OnInteractInput;
        }

        private void OnInteractInput(InputAction.CallbackContext callbackContext) // Called when user press the interact keybind
        {
            if (interactableZoneActive) // If player is in the interact zone (aka in the collider or wtv)
            {
                // Push event
                InteractedWith?.Invoke();
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return; // If object that entered the interactZone isn't player,ignore.
            // update var
            interactableZoneActive = true;
            // Inform listeners that this object is now interactable with player
            InteractableChange?.Invoke(true);
            // Push the interact popup text
            interactText.PushText(popupText,(Vector2)transform.position+offset);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            // If object that exits isn't player, ignore
            if (!other.gameObject.CompareTag("Player")) return;
            // update var
            interactableZoneActive = false;
            // Inform listeners that this object is now no longer interactable with player
            InteractableChange?.Invoke(false);
            // Remove the text popup
            interactText.RemoveText();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white * 0.5f;
            Gizmos.DrawCube(transform.position+(Vector3)offset, new Vector3(2,1));
        }
    }
}