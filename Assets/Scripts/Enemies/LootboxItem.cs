using System;
using Core.Item;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Represents configuration for an item that can be dropped from an enemy
    /// </summary>
    [Serializable]
    public struct LootboxItem
    {
        [Tooltip("The item to drop")]
        public ItemScriptableObject item;
        [Tooltip("Chance for this item to drop"),Range(0,1)]
        public float chance;
    }
}