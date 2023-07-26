using UnityEngine;

namespace Core.Entities
{
    public class ActiveStatusEffect
    {
        public StatusEffect statusEffect;
        public int ticksRemaining;
        public GameObject particlePrefabInstance;
        
        public ActiveStatusEffect(StatusEffect statusEffect, GameObject particlePrefabInstance)
        {
            this.statusEffect = statusEffect;
            this.particlePrefabInstance = particlePrefabInstance;
            ticksRemaining = this.statusEffect.ticks;
        }

        public void TickStart(EntityBody body)
        {
            body.StatsModifiers.Add(new AppliedStatsModifier(this,statusEffect.statsOnce));
            body.Damage(statusEffect.CurrentHealthOnce);
        }
        
        public void Tick(EntityBody body)
        {
            ticksRemaining--;
            body.StatsModifiers.Add(new AppliedStatsModifier(this,statusEffect.statsPerTick));
            body.Damage(statusEffect.CurrentHealthPerTick);
        }

        public void Remove(EntityBody body)
        {
            body.StatusEffects.Remove(this);
            body.StatsModifiers.RemoveAll(modifier => (modifier.owner as ActiveStatusEffect) == this);
            if (particlePrefabInstance != null)
            {
                GameObject.Destroy(particlePrefabInstance);
            }
        }
    }
}