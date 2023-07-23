using Core.Item;
using Player;
using UnityEngine;

namespace Item
{
    [RequireComponent(typeof(ItemPrefabController))]
    public abstract class ItemPrefabScript : MonoBehaviour
    {
        protected ItemPrefabController gateway { get; private set; }
        protected PlayerBody playerBody => gateway.PlayerBody;
        protected Transform player => gateway.Player;
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