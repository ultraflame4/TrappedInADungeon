using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Utils;

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
        /// <param name="handler">The handler</param>
        public void PushInteractText(string description, Vector2 worldPosition, InteractTextHandler handler)
        {
            current = handler;
            string currentBindingName = GameManager.Controls.Player.Interact.GetBindingDisplayString();
            string fullText = $"Press <color=\"yellow\">[{currentBindingName}]</color> to {description}";
            text.text = fullText;
            Rect canvasRect = text.canvas.GetComponent<RectTransform>().rect;
            Vector2 canvasSizeHalf = new Vector2(canvasRect.width, canvasRect.height) / 2;
            Vector2 textSizeHalf = new Vector2(text.preferredWidth, text.preferredHeight) / 2;
            Vector2 minPosition = textSizeHalf - canvasSizeHalf;
            Vector2 maxPosition = textSizeHalf + canvasSizeHalf;
            Vector2 textPos = text.canvas.WorldToCanvasPoint(worldPosition);
            text.rectTransform.anchoredPosition = Vector2.Max(Vector2.Min(textPos, maxPosition), minPosition);
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