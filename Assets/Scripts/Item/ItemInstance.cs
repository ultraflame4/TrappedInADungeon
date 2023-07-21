using Entities;
using UnityEngine;

namespace Item
{
    public class ItemInstance : IEntityStats
    {
        public ItemScriptableObject item { get; private set; }
        public Sprite sprite => item.itemSprite;
        public GameObject prefab => item.itemPrefab;
        // todo implement stats modifiers.
        public ItemInstance(ItemScriptableObject item)
        {
            this.item = item;
        }

        public string GetDisplayName()
        {
            return item.item_name;
        }

        public string GetDisplayDescription()
        {
            return item.item_description;
        }

        public float Attack => item.attack;
        public float Speed => item.speed;
        public float Defense => item.defense;
        public float Health => item.health;
        public float Mana => item.mana;
        public float ManaCost => item.manaCost;
        public float ManaRegen => 0;
    }
}