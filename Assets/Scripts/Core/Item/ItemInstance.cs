using System;
using Core.Entities;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Item
{
    /// <summary>
    /// Represents an instance of an item. This is also used to store the number of items in a stack.
    /// </summary>
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    public class ItemInstance : IEntityStats
    {
        /// <summary>
        /// The item of this instance.
        /// </summary>
        [field: SerializeField] [JsonProperty]
        public ItemScriptableObject item { get; private set; }
        
        /// <summary>
        /// Short hand for item.itemSprite
        /// </summary>
        public Sprite sprite => item.itemSprite;

        /// <summary>
        /// Short hand for item.itemPrefab
        /// </summary>
        public GameObject prefab => item.itemPrefab;

        /// <summary>
        /// Similar items (such as consumables) can be stacked together. This represents the number of items in this stack.
        /// </summary>
        [field: SerializeField] [JsonProperty]
        public int Count { get; private set; } = 1;

        
        public ItemInstance(ItemScriptableObject item)
        {
            this.item = item;
        }

        public string GetDisplayTitle()
        {
            if (Count > 1)
            {
                return $"{item.item_name} x <color=#ff7575>{Count}</color>";
            }
            return  item.item_name;
        }

        public string GetDisplayDescription()
        {
            switch (item.itemType)
            {
                case ItemType.Weapon:
                    return $"{item.item_description}\n<color=\"red\">Atk</color>: {Attack}";
                case ItemType.Skill:
                    return $"{item.item_description}\n<color=#37faf3>Mana</color>: {ManaCost}";
                default:
                    return item.item_description;
            }
        }

        // The stats below may include stats from modifier and hence may differ from the original item stats
        // (Due to time constraints, we are not implementing modifiers and hence these stats are the same as the original item stats)
        public float Attack => item.attack;
        public float Speed => item.speed;
        public float Defense => item.defense;
        public float Health => item.health;
        public float Mana => item.mana;
        public float ManaCost => item.manaCost;
        public float ManaRegen => 0;

        public bool Equals(ItemInstance other)
        {
            // If item is the same, allow them to stack
            return other.item == item &&
                   other.Attack == Attack &&
                   other.Speed == Speed &&
                   other.Defense == Defense &&
                   other.Health == Health &&
                   other.Mana == Mana &&
                   other.ManaCost == ManaCost &&
                   other.ManaRegen == ManaRegen;
        }

        /// <summary>
        /// Combines another item instance with this one.
        /// </summary>
        /// <param name="other">The item instance to combine with.</param>
        /// <returns>true on success, false on failure</returns>
        public bool Combine(ItemInstance other)
        {
            // Weapons cannot stack
            if (item.itemType == ItemType.Weapon) return false;
            // If item is not the same, do not allow them to stack
            if (!Equals(other)) return false;
            // If item is the same, add them to the stack (by increasing the count)
            Count += other.Count;
            return true;
        }

        /// <summary>
        /// DO NOT USE THIS DIRECTLY! Use PlayerInventory.AdjustItemCount instead!
        /// Sets the number of items in this item instance.
        /// </summary>
        /// <param name="count"></param>
        public void _SetCount(int count)
        {
            Count = count;
        }
    }
}