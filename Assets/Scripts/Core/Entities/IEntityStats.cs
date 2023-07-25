namespace Core.Entities
{
    public interface IEntityStats
    {
        public float Attack { get; }
        public float Speed { get; }
        public float Defense { get; }
        public float Health { get; }
    }

    public struct SEntityStats : IEntityStats
    {
        public static SEntityStats Create(IEntityStats stats)
        {
            return new SEntityStats() {
                    Attack = stats.Attack,
                    Speed = stats.Speed,
                    Defense = stats.Defense,
                    Health = stats.Health,
            };
        }
        public float Attack { get; set; }
        public float Speed { get;  set;}
        public float Defense { get;  set;}
        public float Health { get;  set;}
    }
}