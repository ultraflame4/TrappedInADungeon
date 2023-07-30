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
            JObject jsonObj = new() {
                    new JProperty("item_id", value.item_id)
            };
            jsonObj.WriteTo(writer);
        }

        public override ItemScriptableObject ReadJson(JsonReader reader, Type objectType, ItemScriptableObject existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var item = ItemManager.Instance.FindItemById((string)jsonObject["item_id"]);
            if (item == null)
            {
                Debug.LogError($"Could not find item of item_id {item.item_id} in ItemManager!");
            }
            return item;
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;
        
    }
}