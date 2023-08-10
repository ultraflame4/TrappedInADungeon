using System;
using Core.UI;
using Core.Utils;
using PlayerScripts;
using UI.Inventory;
using UnityEngine;

namespace Item
{
    /// <summary>
    /// This component is used to communicate between the item prefab and the hot bar.
    /// </summary>
    public class ItemPrefabController : MonoBehaviour
    {
        /// <summary>
        /// When player use the item (aka press the keybind for the hotbar with this item)
        /// </summary>
        public event Action OnItemUsed;
        /// <summary>
        /// When player release the item (aka release the keybind for the hotbar with this item)
        /// </summary>
        public event Action OnItemReleased;

        /// <summary>
        /// The inventory slot that this item is in.
        /// </summary>
        [HideInInspector]
        public InventorySlot slot;

        [Tooltip("Don't play the itemUseSoundEffect")]
        public bool disableAudio;

        public void UseItem() // Called when the item is to be used
        {
            // If Player doesn't have enough mana to use the item, show error notification
            if (Player.Body.CurrentMana.value < slot.Item.itemInstance.ManaCost) 
            {
                NotificationManager.Instance.PushNotification("Not enough <color=\"blue\">mana</color>!",
                    addData:$"<color=\"yellow\">({Player.Body.CurrentMana.value.ToPrecision(2)}/{slot.Item.itemInstance.ManaCost})</color>");
                return;
            }
            // Apply item usage mana cost
            Player.Body.CurrentMana.value -= slot.Item.itemInstance.ManaCost;
            // If audio has not been disabled, play audio
            if (!disableAudio) slot.Item.itemInstance.item.itemUseSoundEffect?.PlayAtPoint(Player.Transform.position);
            // Invoke item use event so that things like projectiles can be fired.
            OnItemUsed?.Invoke();
        }
        public void ReleaseItem() => OnItemReleased?.Invoke();
    }
}