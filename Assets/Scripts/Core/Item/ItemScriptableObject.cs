using System;
using System.Runtime.Serialization;
using Core.Sound;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Core.Item
{
    /// <summary>
    /// This scriptable object represents items that can be used in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "game_item", menuName = "GameContent/Item"), // Create a menu for this scriptable object
     JsonObject(MemberSerialization.OptIn), // Serialize only the fields marked with [JsonProperty]
     JsonConverter(typeof(ItemScriptableObjectConverter)) // Use custom converter to serialize/deserialize this scriptable object
    ]
    public sealed class ItemScriptableObject : ScriptableObject
    {
        [Tooltip("Unique id for this item"), JsonProperty]
        public string item_id;

        [Tooltip("Name of the item shown in inventory")]
        public string item_name;

        [Tooltip("Description of item shown in inventory"), Multiline(5)]
        public string item_description;

        [Tooltip("Sprite to display in the inventory")]
        public Sprite itemSprite;

        [Tooltip("Type of item")]
        public ItemType itemType;

        [Tooltip("Depends on item type." +
                 "\nWeapon - Weapon Prefab to spawn & use." +
                 "\nProjectile - Projectile prefab to spawn & shoot." +
                 "\nConsumable - Nothing" +
                 "\nCustom - prefab to spawn when item is in hotbar")]
        public GameObject itemPrefab;

        [Tooltip("The amount of damage this item deals when used.")]
        public float attack;

        [Tooltip("The amount of defense this item provides when used.")]
        public float defense;

        [Tooltip("The amount of speed this item provides when used.")]
        public float speed;

        [Tooltip("The amount of health this item provides when used.")]
        public float health;

        [Tooltip("The amount of mana this item provides when used.")]
        public float mana;

        [Tooltip("The amount of mana this item costs when used.")]
        public float manaCost;

        [Tooltip("The sound this item makes when the item is used.")]
        public SoundEffect itemUseSoundEffect = null;
    }
}