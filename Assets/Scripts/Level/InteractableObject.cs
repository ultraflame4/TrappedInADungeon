using System;
using UI;
using UI.InteractText;
using UnityEngine;

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
        public bool interactableZoneActive { get; private set; }

        private CircleCollider2D circleCollider2D;
        private uint popupTextId;
        private InteractTextHandler interactText;
        private void Awake()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
            interactText = InteractTextManager.Instance.Create();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            interactableZoneActive = true;
            InteractableChange?.Invoke(true);
            Debug.Log($" interactText null? {interactText == null}. Manager null? {interactText?.manager == null}");
            interactText.PushText(popupText,(Vector2)transform.position+offset);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            interactableZoneActive = false;
            InteractableChange?.Invoke(false);
            interactText.RemoveText();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white * 0.5f;
            Gizmos.DrawCube(transform.position+(Vector3)offset, new Vector3(2,1));
        }
    }
}