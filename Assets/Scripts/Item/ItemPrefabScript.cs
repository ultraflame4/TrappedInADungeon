using Core.Item;
using PlayerScripts;
using UnityEngine;

namespace Item
{
    /// <summary>
    /// Base class for controller scripts in item prefabs
    /// </summary>
    [RequireComponent(typeof(ItemPrefabController))]
    public abstract class ItemPrefabScript : MonoBehaviour
    {
        protected ItemPrefabController gateway { get; private set; }
        
        protected ItemInstance itemInstance => gateway.slot.Item.itemInstance;

        void Start()
        {
            gateway = GetComponent<ItemPrefabController>();
            gateway.OnItemUsed += OnItemUse;
            gateway.OnItemReleased += OnItemReleased;
        }

        protected abstract void OnItemUse();
        protected virtual void OnItemReleased(){}
    }
}