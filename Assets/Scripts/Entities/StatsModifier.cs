namespace Entities
{
    public class StatsModifier: IEntityStats, IItemStats
    {
        public float Attack { get; set; }
        public float Speed { get; set; }
        public float Defense { get; set; }
        public float Health { get; set; }
        public float Stamina { get; set; }
        public float Mana { get; set; }
        public float ManaCost { get; set; }
    }
}