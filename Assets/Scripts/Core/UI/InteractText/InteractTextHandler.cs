using UnityEngine;

namespace Core.UI.InteractText
{
    /// <summary>
    /// Helper class for pushing interact text to the interact text manager.
    /// </summary>
    public class InteractTextHandler
    {
        // The manager that this handler is associated with
        private InteractTextManager manager;
        public InteractTextHandler(InteractTextManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Pushes interactation text to the manager.
        /// </summary>
        /// <param name="description">The text to display.</param>
        /// <param name="worldPosition">World position to display the text</param>
        public void PushText(string description, Vector2 worldPosition)
        {
            // Push the text to the manager
            manager.PushInteractText(description, worldPosition, this);
        }

        public void RemoveText()
        {
            // Remove the text from the manager
            manager.RemoveInteractText(this);
        }        
    }
}