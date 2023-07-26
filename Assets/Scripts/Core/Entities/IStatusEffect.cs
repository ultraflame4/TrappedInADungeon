
namespace Core.Entities
{
    public interface  IStatusEffect
    {
        /// <summary>
        /// Duration of status effect in ticks
        /// </summary>
        public int TicksLeft { get; set; }

        public void EffectStart(EntityBody body);
        public void EffectEnd(EntityBody body);
        public void Tick(EntityBody body);
    }
}