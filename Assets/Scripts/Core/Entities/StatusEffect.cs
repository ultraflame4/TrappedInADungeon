
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Entities
{
    
    [CreateAssetMenu(fileName = "StatusEffect", menuName = "GameContent/StatusEffect", order = 0)]
    public class  StatusEffect : ScriptableObject
    {
        [Tooltip("Duration of status effect in ticks")]
        public int ticks;

        [Tooltip("These stats are only applied once"),SerializeField]
        public StatsModifier statsOnce = new ();

        [Tooltip("Modification to the current health of the entity applied once")]
        public float CurrentHealthOnce;

        [Tooltip("These stats are applied multiple times over time."),SerializeField]
        public StatsModifier statsPerTick= new ();

        [Tooltip("Modification to the current health of the entity applied over time")]
        public float CurrentHealthPerTick;
        [Tooltip("Particle prefab to use when this effect is active")]
        public GameObject particleEffect;
        [Tooltip("Whether this effect will stun the enemy when dealing damage.")]
        public bool stun = true;

    }
}