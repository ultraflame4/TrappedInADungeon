using UnityEngine;

namespace Core.Entities
{
    /// <summary>
    /// Represents a status effect that is being applied to an entity
    /// </summary>
    public class ActiveStatusEffect
    {
        public StatusEffect statusEffect; // The status effect that is being applied
        public int ticksRemaining; // Remaining duration of the status effect
        public GameObject particlePrefabInstance; // The particle effect that is being played for this status effect
        
        public ActiveStatusEffect(StatusEffect statusEffect, GameObject particlePrefabInstance)
        {
            this.statusEffect = statusEffect;
            this.particlePrefabInstance = particlePrefabInstance;
            ticksRemaining = this.statusEffect.ticks;
        }

        public void TickStart(EntityBody body)
        {
            // Apply any stats modifiers from the status effect
            body.StatsModifiers.Add(new AppliedStatsModifier(this,statusEffect.statsOnce));
            // Apply status effect start damage.
            body.DamageRaw(statusEffect.CurrentHealthOnce,statusEffect.stun); 
        }
        
        public void Tick(EntityBody body)
        {
            // Reduce duration
            ticksRemaining--;
            // Apply any stats modifiers from the status effect
            body.StatsModifiers.Add(new AppliedStatsModifier(this,statusEffect.statsPerTick));
            // Apply status effect damage.
            body.DamageRaw(statusEffect.CurrentHealthPerTick,statusEffect.stun);
        }

        public void Remove(EntityBody body)
        {
            // Remove itself
            body.StatusEffects.Remove(this);
            // Remove all stats modifiers that were applied by this status effect
            body.StatsModifiers.RemoveAll(modifier => (modifier.owner as ActiveStatusEffect) == this);
            // If there is a particle effect, destroy it
            if (particlePrefabInstance != null)
            {
                GameObject.Destroy(particlePrefabInstance);
            }
        }
    }
}