namespace Core.Entities
{
    // Additional stats for magic (skills)
    public interface IMagicStats
    {
        public float Mana { get; }
        public float ManaRegen { get; }
    }
}