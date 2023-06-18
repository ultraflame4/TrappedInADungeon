using UnityEngine;

namespace UI.Dragging
{
    /// <summary>
    /// interface for all data that needs to be passed to the drop target when dragging
    /// </summary>
    public abstract class DragEventData
    {
        public abstract Sprite GetSprite();
    }
}