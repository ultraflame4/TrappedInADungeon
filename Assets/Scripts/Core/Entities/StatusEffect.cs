
using UnityEngine;

namespace Core.Entities
{
    
    [CreateAssetMenu(fileName = "StatusEffect", menuName = "GameContent/StatusEffect", order = 0)]
    public class  StatusEffect : ScriptableObject
    {
        public string status_id;
        /// <summary>
        /// Duration of status effect in ticks
        /// </summary>
        public int ticks;

        /// <summary>
        /// These stats are only applied once
        /// </summary>
        public StatsModifier statsOnce;

        /// <summary>
        /// These stats are applied multiple times over time.
        /// </summary>
        public StatsModifier statsOverTime;
    }
}