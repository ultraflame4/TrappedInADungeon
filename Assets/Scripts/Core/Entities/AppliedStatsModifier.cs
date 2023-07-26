namespace Core.Entities
{
    public class AppliedStatsModifier
    {
        public object owner;
        public StatsModifier statsModifier;
        public AppliedStatsModifier(object owner, StatsModifier statsModifier)
        {
            this.owner = owner;
            this.statsModifier = statsModifier;
        }
    }
}