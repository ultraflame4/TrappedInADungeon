using System;
using System.Collections;
using Core.Item;
using Core.Save;
using Core.UI;
using Newtonsoft.Json;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    /// <summary>
    /// Represents an item in the inventory list UI
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InventoryListItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Button deleteBtn;
        public Image itemImage;
        public Image focusedOutline;
        public bool IsFocused => focusedOutline.enabled;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public InvSlotItemInstance itemInstance { get; private set; }
        private bool isHovered;


        
        public void OnDeleteBtnClick() 
        {
            // When delete button is clicked, remove item from inventory
            Player.Inventory.RemoveItem(itemInstance.itemInstance);
        }

        /// <summary>
        /// Sets the item instance this list item is showing
        /// </summary>
        /// <param name="item"></param>
        public void SetItem(ItemInstance item)
        {
            // Set the item sprite, item title, and item description
            itemImage.sprite = item.sprite;
            title.text = item.GetDisplayTitle();
            description.text = item.GetDisplayDescription();
            // Create a new InvSlotItemInstance for this item
            itemInstance = new InvSlotItemInstance(item);
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // When drag begins, tell the cursor controller to start dragging this item
            CursorController.Instance.StartDrag(itemInstance, itemInstance.itemInstance.sprite);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // When drag ends, tell the cursor controller to end drag
            CursorController.Instance.EndDrag();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // When pointer enters, set hovered to true
            isHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // When pointer enters, set hovered to false
            isHovered = false;
        }

        private void Update()
        {
            // If this item is not active, return
            if (!isActiveAndEnabled) return;
            
            // If the mouse is clicked, and this item is hovered, focus this item
            if (GameManager.Controls.Menus.MouseClick.WasPressedThisFrame())
            {
                if (isHovered)
                {
                    SetFocused(true);
                }
                else // if clicked outside, defocus
                {
                    SetFocused(false);
                }
            }
            // If the mouse is released outside of this item, defocus this item
            else if (GameManager.Controls.Menus.MouseClick.WasReleasedThisFrame() && !isHovered)
            {
                SetFocused(false);
            }
        }

        /// <summary>
        /// Focuses or defocuses this item
        /// </summary>
        /// <param name="value"></param>
        void SetFocused(bool value)
        {
            // When focused, show the focused outline, vice versa
            focusedOutline.enabled = value;
            // When focused, mark the item instance as focused, vice versa
            itemInstance.focused = value;
            // When focused, show the delete button, vice versa
            deleteBtn.gameObject.SetActive(value);
        }

        private void OnDisable()
        {
            // When disabled, manually defocus this item
            if (focusedOutline != null) focusedOutline.enabled = false;
            if (itemInstance != null) itemInstance.focused = false;
        }
    }
}