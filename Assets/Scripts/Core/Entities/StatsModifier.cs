using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Entities
{
    [JsonObject(MemberSerialization.Fields),Serializable]
    public class StatsModifier: IEntityStats, IItemStats, IMagicStats
    {
        [field: SerializeField]
        public float Attack { get; set; }
        [field: SerializeField]
        public float Speed { get; set; }
        [field: SerializeField]
        public float Defense { get; set; }
        [field: SerializeField]
        public float Health { get; set; }
        [field: SerializeField]
        public float Mana { get; set; }
        [field: SerializeField]
        public float ManaCost { get; set; }
        [field: SerializeField]
        public float ManaRegen { get; set; }
    }
}