using System;
using Core.UI;
using Core.Utils;
using PlayerScripts;
using UI.Inventory;
using UnityEngine;

namespace Item
{
    /// <summary>
    /// This component is used to communicate between the item prefab and the hotbar.
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



        public void UseItem()
        {
            if (PlayerScripts.Player.Body.CurrentMana.value < slot.Item.itemInstance.ManaCost)
            {
                NotificationManager.Instance.PushNotification("Not enough <color=\"blue\">mana</color>!",
                    addData:$"<color=\"yellow\">({PlayerScripts.Player.Body.CurrentMana.value.ToPrecision(2)}/{slot.Item.itemInstance.ManaCost})</color>");
                return;
            }
            PlayerScripts.Player.Body.CurrentMana.value -= slot.Item.itemInstance.ManaCost;
            
            OnItemUsed?.Invoke();
        }
        public void ReleaseItem() => OnItemReleased?.Invoke();
    }
}