using TMPro;
using UnityEngine;

namespace UI.InteractText
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class InteractTextManager : MonoBehaviour
    {
        private TextMeshProUGUI text;
        public string currentText { get; private set; }
        private InteractTextHandler current;
        public static InteractTextManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Warning: multiple instances of InteractTextManager found! The static instance will be changed to this one!!!! This is probably not what you want!");
            }

            Instance = this;
        }

        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// Displays a "Press [E] to insert description or something
        /// For example:
        /// <example>
        /// <code>
        /// PushInteractText("Open Chest",transform.position);
        /// </code>
        /// Will show "Press [E] to Open Chest".
        /// The actual message shown may differ depending on config.
        /// </example>
        /// </summary>
        /// <param name="description">The interaction text / description</param>
        /// <param name="worldPosition">The position to display the text (in world space)</param>
        public void PushInteractText(string description, Vector2 worldPosition, InteractTextHandler handler)
        {
            string fullText = $"Press <color=\"yellow\">E</color> to {description}";
            text.text = fullText;
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
            text.rectTransform.anchoredPosition = screenPoint - text.canvas.GetComponent<RectTransform>().sizeDelta / 2f;
            return;
        }

        public void RemoveInteractText(InteractTextHandler handler)
        {
            if (current != handler) return;
            text.text = "";
        }

        /// <summary>
        /// Creates and returns a InteractTextHandler object to easily push and remove text
        /// </summary>
        public InteractTextHandler Create()
        {
            return new InteractTextHandler(this);
        }
    }
}