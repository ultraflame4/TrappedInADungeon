using System;
using Item;
using UnityEngine;

namespace Consumable
{
    public class ConsumableItemController : ItemPrefabScript
    {

        protected override void OnItemUse()
        {
            playerBody.CurrentHealth.value += itemInstance.item.health;
            playerBody.CurrentMana.value += itemInstance.item.mana;
            gateway.PlayerInventory.AdjustItemCount(itemInstance,itemInstance.Count-1);
        }
    }
}