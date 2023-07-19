using UnityEngine;

namespace UI.InteractText
{
    public class InteractTextHandler
    {
        public InteractTextManager manager;
        public InteractTextHandler(InteractTextManager manager)
        {
            this.manager = manager;
        }

        public void PushText(string description, Vector2 worldPosition)
        {
            manager.PushInteractText(description, worldPosition, this);
        }

        public void RemoveText()
        {
            manager.RemoveInteractText(this);
        }        
    }
}