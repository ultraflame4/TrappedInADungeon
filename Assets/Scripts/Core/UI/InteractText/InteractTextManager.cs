using Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.UI.InteractText
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class InteractTextManager : MonoBehaviour
    {
        // refernce to The text mesh pro component
        private TextMeshProUGUI text;
        // The current handler that is displaying text
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
            text.text = ""; // Clear the text shown
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
            // Get the current key binding name for the interact action
            string currentBindingName = GameManager.Controls.Player.Interact.GetBindingDisplayString();
            // Create the full text to display
            string fullText = $"Press <color=\"yellow\">[{currentBindingName}]</color> to {description}";
            text.text = fullText; // Set the text to display
            // Size of canvas
            Rect canvasRect = text.canvas.GetComponent<RectTransform>().rect;
            // Half the size of the canvas and the text
            Vector2 canvasSizeHalf = new Vector2(canvasRect.width, canvasRect.height) / 2;
            Vector2 textSizeHalf = new Vector2(text.preferredWidth, text.preferredHeight) / 2;
            
            // Calculate the min and max position for the text so that it does not appear off the screen
            Vector2 minPosition = textSizeHalf - canvasSizeHalf + Vector2.right*10; // Vector2.right * 10 to add some padding to the edge of the screen
            Vector2 maxPosition = textSizeHalf + canvasSizeHalf + Vector2.left*10; // Vector2.left * 10 to add some padding to the edge of the screen
            // Convert the world position to a canvas position
            Vector2 textPos = text.canvas.WorldToCanvasPoint(worldPosition);
            // Set the text position to the clamped position
            text.rectTransform.anchoredPosition = Vector2.Max(Vector2.Min(textPos, maxPosition), minPosition);
        }
        
        /// <summary>
        /// Removes the current interact text from the screen.
        /// Does nothing if the current handler is not the same as the one passed in.
        /// </summary>
        /// <param name="handler"></param>
        public void RemoveInteractText(InteractTextHandler handler)
        {
            // Only remove the text if the current handler is the same as the one passed in!
            if (current != handler) return;
            current = null;
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