﻿using System;
using Core.Entities;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Item
{
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    public class ItemInstance : IEntityStats
    {
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
        /// Similar items (such as consumables) can be stacked together.
        /// </summary>
        [field: SerializeField] [JsonProperty]
        public int Count { get; private set; } = 1;

        // todo implement stats modifiers.
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
            if (item.itemType == ItemType.Weapon) return false;
            if (!Equals(other)) return false;
            Count += other.Count;
            return true;
        }

        /// <summary>
        /// DO NOT USE THIS DIRECTLY! Use PlayerInventory.AdjustItemCount instead!
        /// Sets the number of items in this instance.
        /// </summary>
        /// <param name="count"></param>
        public void _SetCount(int count)
        {
            Count = count;
        }
    }
}