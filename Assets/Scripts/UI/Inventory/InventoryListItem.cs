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
            Player.Inventory.RemoveItem(itemInstance.itemInstance);
        }

        /// <summary>
        /// Sets the item instance this list item is showing
        /// </summary>
        /// <param name="item"></param>
        public void SetItem(ItemInstance item)
        {
            itemImage.sprite = item.sprite;
            title.text = item.GetDisplayTitle();
            description.text = item.GetDisplayDescription();
            itemInstance = new InvSlotItemInstance(item);
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CursorController.Instance.StartDrag(itemInstance,itemInstance.itemInstance.sprite);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            CursorController.Instance.EndDrag();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
        }

        private void Update()
        {
            if (!isActiveAndEnabled) return;
            if (GameManager.Controls.Menus.MouseClick.WasPressedThisFrame())
            {
                if (isHovered)
                {
                    SetFocused(true);
                }
                else// if clicked outside, defocus
                {
                    SetFocused(false);
                }
            }
            else if (GameManager.Controls.Menus.MouseClick.WasReleasedThisFrame() && !isHovered) // If release outside, defocus
            {
                SetFocused(false);
            }
        }

        void SetFocused(bool value)
        {

            focusedOutline.enabled = value;
            itemInstance.focused = value;
            deleteBtn.gameObject.SetActive(value);
        }

        private void OnDisable()
        {
            if (focusedOutline!=null) focusedOutline.enabled = false;
            if (itemInstance!=null) itemInstance.focused = false;
        }
    }
}