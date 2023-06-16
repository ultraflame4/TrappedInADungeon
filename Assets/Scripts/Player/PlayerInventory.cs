using System;
using System.Collections.Generic;
using System.Linq;
using Item;

namespace Player
{
    public class PlayerInventory
    {
        private List<ItemInstance> items = new List<ItemInstance>();
        public event Action inventoryUpdate;
        
        /// <summary>
        /// Adds an item instance to the inventory
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public void AddItem<T>(T item) where T : ItemInstance
        {
            items.Add(item);
            inventoryUpdate?.Invoke();
        }
        /// <summary>
        /// Removes the specific item instance
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public void RemoveItem<T>(T item) where T : ItemInstance
        {
            items.Remove(item);
            inventoryUpdate?.Invoke();
        }
        /// <summary>
        /// Removes an item instance by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The item instance removed from the inventory</returns>
        public ItemInstance RemoveAt(int index)
        {
            var obj = items[index];
            items.RemoveAt(index);
            inventoryUpdate?.Invoke();
            return obj;
        }

        /// <summary>
        /// Returns all item instances in the inventory that matches type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetAllItemOfType<T>() where T : ItemInstance
        {
            return items.Where(x => x is T).Cast<T>().ToArray();
        }

        public void Clear()
        {
            items.Clear();
            inventoryUpdate?.Invoke();
        }
    }
}