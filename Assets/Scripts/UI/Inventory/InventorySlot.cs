using System;
using System.ComponentModel;
using Core.Item;
using Core.Save;
using Core.UI;
using Core.Utils;
using EasyButtons;
using Item;
using Newtonsoft.Json;
using PlayerScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Inventory
{
    /// <summary>
    /// Represents a slot in the inventory UI. Such as the weapon slot, or the skill slot.
    /// </summary>
    public class InventorySlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, ISaveHandler
    {
        // Component references
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

        [Tooltip("Index of this slot")]
        public int slotIndex;

        /// <summary>
        /// This event is fired whenever the item in this slot changes. The value passed is the new item instance or null (if cleared).
        /// </summary>
        public event Action<ItemInstance> onItemChanged;

        /// <summary>
        /// This event is fired whenever the item in this slot is used. The value passed is the current item instance or null (if empty).
        /// </summary>
        public event Action<ItemInstance> onItemUsed;

        // references
        private InputAction inputAction;
        private InvSlotItemInstance currentItem = null;
        private ItemPrefabController itemGateway = null;
        public InvSlotItemInstance Item => currentItem;

        /// <summary>
        /// Index of the current item in the inventory,  Mainly used for serialising to and from json
        /// </summary>
        [JsonProperty]
        private int CurrentItemInventoryIndex = -1;

        [JsonProperty]
        private ItemInstance CurrentItemName => currentItem?.itemInstance;

        private void Awake()
        {
            GameSaveManager.AddSaveHandler($"inventory.slot_{slotIndex}", this);
        }

        private void Start()
        {
            // We need to find the input action from the instance of GameControls
            inputAction = GameManager.Controls.FindAction(inputRef.action.id.ToString(), true);
            // Register for inventory update events
            Player.Inventory.InventoryUpdate += FirstInventoryUpdate;
            Player.Inventory.InventoryUpdate += OnInventoryUpdate;
        }

        void FirstInventoryUpdate()
        {
            // On first inventory update, if there is a CurrentItemInventoryIndex stored in the save,
            // retrieve the item from the inventory and set it as the current item.
            if (CurrentItemInventoryIndex < 0) return;
            SetItem(InventoryPanel.Instance.GetInvItemByIndex(CurrentItemInventoryIndex));
            // Remove this listener so that it only runs once
            Player.Inventory.InventoryUpdate -= FirstInventoryUpdate;
        }

        void OnInventoryUpdate()
        {
            // If the item instance has been removed from the inventory, set this slot to empty.
            if (!Player.Inventory.Contains(currentItem?.itemInstance)) SetItem(null);
        }

        void Update()
        {
            if (inputAction is not null)
            {
                if (inputAction.WasPressedThisFrame()) // If slot is active
                {
                    // only activate if player is not pointing at ui
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        // override the sprite button's active state so that it appears to be active when item is used
                        spriteButton.activeOverride = true;
                        if (itemGateway) itemGateway.UseItem();
                        onItemUsed?.Invoke(currentItem.itemInstance);
                        spriteButton.UpdateImageSprite();
                    }
                    else
                    {
                        // Get the currently focused item and set the item in this slot to that item.
                        var focused = InventoryPanel.Instance.GetFocused();
                        if (focused != null)
                        {
                            SetItem(focused);
                        }
                    }
                }

                // If keybind for slot is released, call the release item method on the item gateway
                if (inputAction.WasReleasedThisFrame())
                {
                    if (itemGateway) itemGateway.ReleaseItem();
                    // stop overriding the active state of the button
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
        public bool SetItem(InvSlotItemInstance item)
        {
            // If item is null, set the item to null (clear the slot)
            if (item == null)
            {
                _SetItem(null);
                return true;
            }

            // If item is weapon, check if this slot is a weapon slot, if not, return false (and don't set the item)
            if (item.itemInstance.item.itemType == ItemType.Weapon)
            {
                if (!isWeaponSlot) return false;
                _SetItem(item);
                return true;
            }

            // If item is not weapon, check if this slot is a weapon slot, if so, return false (and don't set the item)
            if (isWeaponSlot) return false;
            // if passed all checks, set the item
            _SetItem(item);
            return true;
        }

        /// <summary>
        /// Raw method to set item in this slot. Does not do any checks on the item type.
        /// </summary>
        /// <param name="item"></param>
        private void _SetItem(InvSlotItemInstance item)
        {
            if (currentItem == item) return; // If item instance is the same in this slot, do nothing
            if (currentItem != null) // If slot has existing item, clear it
            {
                currentItem.assignedSlot = null; // First clear the reference to this slot
                if (itemGateway != null) Destroy(itemGateway.gameObject); // Then destroy the item prefab object
            }

            if (item != null) // If new item is not null, configure the new item
            {
                // Create the item prefab for the item
                if (item.itemInstance.prefab != null)
                {
                    var obj = Instantiate(item.itemInstance.prefab);

                    // Update the item gateway slot reference
                    itemGateway = obj.GetComponent<ItemPrefabController>();
                    if (itemGateway == null)
                    {
                        Debug.Log($"Prefab  {currentItem.itemInstance.prefab} does not have ItemPrefabController component!");
                    }
                    else
                    {
                        itemGateway.slot = this;
                    }
                }
                
                item.assignedSlot?.SetItem(null); // If new item is already in a slot, clear that slot first
                item.assignedSlot = this; // Set reference (for new item) before setting this slot
            }

            // Set the current item, inventory index and update the item image
            currentItem = item;
            CurrentItemInventoryIndex = Player.Inventory.IndexOf(currentItem?.itemInstance);
            itemImage.SetSprite(currentItem?.itemInstance.sprite);
            // Invoke the item changed event
            onItemChanged?.Invoke(currentItem?.itemInstance);
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // dont allow dragging if there is no item in this slot
            if (currentItem == null) return;
            // hide item image
            itemImage.enabled = false;
            CursorController.Instance.StartDrag(this, currentItem.itemInstance.sprite);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // if the item was not dropped in another slot, count as user throwing the item into the void
            if (!CursorController.Instance.optionalDropSuccess)
            {
                SetItem(null);
            }

            // show item image if there is an item in this slot
            itemImage.enabled = currentItem != null;
            CursorController.Instance.EndDrag();
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggedData = CursorController.Instance.GetDraggedData();
            if (draggedData is InvSlotItemInstance item)
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
                CursorController.Instance.optionalDropSuccess = true;
            }
        }
    }
}