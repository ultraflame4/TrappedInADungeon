using System;
using System.ComponentModel;
using Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils;

namespace UI.Inventory
{
    public class InventorySlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        public Image itemImage;
        public SpriteButton spriteButton;

        /// <summary>
        /// The input button name to use for this slot. If this is set, the slot will be activated when the button is pressed.
        /// </summary>
        // public string buttonName = "";
        public InputActionReference inputRef;

        /// <summary>
        /// Set this to true if this inventory slot is meant to hold weapons.
        /// Weapon slots can only hold weapons. non-Weapon slots can hold anything except weapons.
        /// </summary>
        public bool isWeaponSlot = false;

        /// <summary>
        /// This event is fired whenever the item in this slot changes. The value passed is the new item instance or null (if cleared).
        /// </summary>
        public event Action<ItemInstance> onItemChanged;

        /// <summary>
        /// This event is fired whenever the item in this slot is used. The value passed is the current item instance or null (if empty).
        /// </summary>
        public event Action<ItemInstance> onItemUsed;

        [ReadOnly(true)] public InputAction inputAction;
        [ReadOnly(true)] public int slotIndex;
        private InventoryItemInstance currentItem = null;
        private ItemPrefabController itemGateway = null;

        public InventoryItemInstance Item => currentItem;

        private void Start()
        {
            // We need to find the input action from the instance of GameControls
            inputAction = GameManager.Controls.FindAction(inputRef.action.id.ToString(), true);
        }

        void Update()
        {
            if (inputAction is not null)
            {
                if (inputAction.WasPerformedThisFrame())
                {
                    // only activate if player is not pointing at ui
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        spriteButton.activeOverride = true;
                        if (itemGateway) itemGateway.UseItem();
                        onItemUsed?.Invoke(currentItem.itemInstance);
                        spriteButton.UpdateImageSprite();
                    }
                }

                if (inputAction.WasReleasedThisFrame())
                {
                    if (itemGateway) itemGateway.ReleaseItem();
                    spriteButton.activeOverride = false;
                    spriteButton.UpdateImageSprite();
                }
            }
        }

        /// <summary>
        /// Sets the current item instance in this slot.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Returns true if successful, vice versa</returns>
        public bool SetItem(InventoryItemInstance item)
        {
            if (item == null)
            {
                _SetItem(null);
                return true;
            }
            
            if (item.itemInstance.item.itemType == ItemType.Weapon)
            {
                if (!isWeaponSlot) return false;
                _SetItem(item);
                return true;
            }

            if (isWeaponSlot) return false;
            _SetItem(item);
            return true;
        }

        private void _SetItem(InventoryItemInstance item)
        {
            if (currentItem == item) return; // If item instance is the same in this slot, do nothing
            if (item is null) // If clearing this slot
            {
                currentItem.assignedSlot = null; // First clear the reference to this slot
                if (itemGateway != null) Destroy(itemGateway.gameObject); // Then destroy the item prefab object
            }
            else
            {
                // Instantiate the item prefab
                if (item.itemInstance.prefab != null)
                {
                    var obj = Instantiate(item.itemInstance.prefab);
                    itemGateway = obj.GetComponent<ItemPrefabController>();
                    if (itemGateway is null)
                    {
                        Debug.Log($"Prefab  {currentItem.itemInstance.prefab} does not have ItemPrefabController component!");
                        Destroy(obj);
                        return; // Cannot instantiate item prefab, do nothing
                    }
                    itemGateway.slot = this;
                }

                item.assignedSlot?.SetItem(null); // If new item is already in a slot, clear that slot first
                item.assignedSlot = this; // Set reference (for new item) before setting this slot
            }

            currentItem = item;
            itemImage.SetSprite(currentItem?.itemInstance.sprite);
            onItemChanged?.Invoke(currentItem?.itemInstance);
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // dont allow dragging if there is no item in this slot
            if (currentItem == null) return;
            // hide item image
            itemImage.enabled = false;
            CursorController.GetInstance().StartDrag(this, currentItem.itemInstance.sprite);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // if the item was not dropped in another slot, count as user throwing the item into the void
            if (!CursorController.GetInstance().optionalDropSuccess)
            {
                SetItem(null);
            }

            // show item image if there is an item in this slot
            itemImage.enabled = currentItem != null;
            CursorController.GetInstance().EndDrag();
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggedData = CursorController.GetInstance().GetDraggedData();
            if (draggedData is InventoryItemInstance item)
            {
                SetItem(item);
            }
            else if (draggedData is InventorySlot inventorySlot)
            {
                item = inventorySlot.currentItem; // get item from the other slot
                if (SetItem(item)) // if successfully set item
                {
                    inventorySlot.SetItem(null); // clear the item in the other slot
                }

                // Tell the other slot that the item was dropped in another slot (regardless of whether the inventorySlot.SetItem() was successful
                CursorController.GetInstance().optionalDropSuccess = true;
            }
        }
    }
}