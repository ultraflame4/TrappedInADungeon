using System;
using Core.Item;
using Item;
using UnityEngine;

namespace Consumable
{
    public class ConsumableItemController : ItemPrefabScript
    {

        protected override void OnItemUse()
        {
            playerBody.CurrentHealth.value += itemInstance.item.health/100*playerBody.Health;
            playerBody.CurrentMana.value += itemInstance.item.mana/100*playerBody.Mana;
            gateway.PlayerInventory.AdjustItemCount(itemInstance,itemInstance.Count-1);
        }
    }
}