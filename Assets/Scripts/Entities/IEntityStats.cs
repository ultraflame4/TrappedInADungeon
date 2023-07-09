namespace Entities
{
    public interface IEntityStats
    {
        public float Attack { get; }
        public float Speed { get; }
        public float Defense { get; }
        public float Health { get; }
        public float Stamina { get; }
        public float Mana { get; }
    }
}