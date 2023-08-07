using System;
using Core.Item;
using Item;
using PlayerScripts;
using UnityEngine;

namespace Consumable
{
    /// <summary>
    /// Controller for consumable items
    /// </summary>
    public class ConsumableItemController : ItemPrefabScript
    {

        protected override void OnItemUse()
        {
            // Add health and mana to player according to the consumable item stats
            Player.Body.CurrentHealth.value += itemInstance.item.health/100*Player.Body.Health; // Increase health by percentage of max health
            Player.Body.CurrentMana.value += itemInstance.item.mana/100*Player.Body.Mana; // Increase mana by percentage of max mana
            Player.Inventory.AdjustItemCount(itemInstance,itemInstance.Count-1); // Decrease item count by 1
        }
    }
}