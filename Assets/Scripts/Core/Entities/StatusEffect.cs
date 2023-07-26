namespace Core.Entities
{
    public abstract class StatusEffect
    {
        public int ticksLeft;
        public abstract void EffectStart(EntityBody body);
        public abstract void EffectEnd(EntityBody body);
        public abstract void Tick(EntityBody body);
    }
}