using System;
using Core.Item;
using Item;
using PlayerScripts;
using UnityEngine;

namespace Consumable
{
    public class ConsumableItemController : ItemPrefabScript
    {

        protected override void OnItemUse()
        {
            Player.Body.CurrentHealth.value += itemInstance.item.health/100*Player.Body.Health;
            Player.Body.CurrentMana.value += itemInstance.item.mana/100*Player.Body.Mana;
            Player.Inventory.AdjustItemCount(itemInstance,itemInstance.Count-1);
        }
    }
}