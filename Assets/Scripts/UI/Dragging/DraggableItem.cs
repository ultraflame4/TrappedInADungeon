using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Dragging
{
    /// <summary>
    /// A helper class for managing dragging of items.<br/>
    /// Drag and drop to be implemented by the inheriting class. ( I wld have used IDropHandler but it's not working for some reason). <br/>
    ///
    /// Usage:<br/>
    ///  1. The item that can be dragged should inherit this class.<br/>
    ///  3. The game object where the item can be dropped should have a component of type C to manage the dropping and transfer or data.<br/>
    ///     This game object is referred to as the "slot" in this class.<br/>
    /// </summary>
    /// <typeparam name="C">The component of the slot to retrieve</typeparam>
    public abstract class DraggableItem<C> : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private bool dragAllowed = false;
        public void OnDrag(PointerEventData eventData) { }

        public void OnBeginDrag(PointerEventData eventData)
        {
            dragAllowed= AllowDrag();
            if (!dragAllowed) return;
            CursorController.GetInstance().SetSprite(GetDragCursorSprite());
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!dragAllowed) return;
            List<RaycastResult> resultList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, resultList);
            // Get the first component of type C from the list of raycast results
            C slot = resultList.Select(x => x.gameObject.GetComponent<C>()).Where(x => x is not null).FirstOrDefault();
            if (slot is not null)
            {
                DropOnSlot(slot);
            } 
            CursorController.GetInstance().SetSprite(null);
            dragAllowed= false;
        }

        /// <summary>
        /// Returns the sprite to be used as the cursor when dragging
        /// </summary>
        /// <returns></returns>
        public abstract Sprite GetDragCursorSprite();
        /// <summary>
        /// Called when the item is dropped on a slot
        /// </summary>
        /// <param name="slot">The slot the item was dropped on</param>
        public abstract void DropOnSlot(C slot);
        /// <summary>
        /// Optional. Override this to set additional conditions for dragging to start
        /// </summary>
        /// <returns></returns>
        public virtual bool AllowDrag() => true;
    }
}