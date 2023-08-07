using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Core.Item
{
    /// <summary>
    /// This class handles the conversion of ItemScriptableObject to and from json 
    /// </summary>
    public class ItemScriptableObjectConverter : JsonConverter<ItemScriptableObject>
    {
        public override void WriteJson(JsonWriter writer, ItemScriptableObject value, JsonSerializer serializer)
        {
            // Serialize only the item_id
            JObject jsonObj = new() { new JProperty("item_id", value.item_id) };
            jsonObj.WriteTo(writer);
        }

        public override ItemScriptableObject ReadJson(JsonReader reader, Type objectType, ItemScriptableObject existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // --- Read the json and find the item in ItemManager using item_id ---
            // Convert the json reader to a JObject so we can access the item_id
            var jsonObject = JObject.Load(reader);
            // Find the item in the ItemManager using the item_id in jsonObject
            var item = ItemManager.Instance.FindItemById((string)jsonObject["item_id"]);
            // Log an error if the item could not be found
            if (item == null) Debug.LogError($"Could not find item of item_id {item.item_id} in ItemManager!");
            // Return the item or null if it could not be found
            return item;
        }

        // Configuration for this converter
        public override bool CanRead => true;
        public override bool CanWrite => true;
        
    }
}