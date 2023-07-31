namespace Core.Entities
{
    /// <summary>
    /// Represents a StatsModifier that is being applied to an entity
    /// </summary>
    public class AppliedStatsModifier
    {
        /// <summary>
        /// The object that applied this stats modifier.
        /// Useful when removing the modifier.
        /// </summary>
        public object owner;
        /// <summary>
        /// The stats to modify
        /// </summary>
        public StatsModifier statsModifier;
        public AppliedStatsModifier(object owner, StatsModifier statsModifier)
        {
            this.owner = owner;
            this.statsModifier = statsModifier;
        }
    }
}