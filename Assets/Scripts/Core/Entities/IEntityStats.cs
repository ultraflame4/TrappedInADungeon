namespace Core.Entities
{
    /// <summary>
    /// Interface for common stats shared between entities
    /// </summary>
    public interface IEntityStats
    {
        public float Attack { get; }
        public float Speed { get; }
        public float Defense { get; }
        public float Health { get; }
    }

    /// <summary>
    /// A very useful struct that implements IEntityStats and ease in doing some operations
    /// </summary>
    public struct SEntityStats : IEntityStats
    {
        /// <summary>
        /// Creates a new SEntityStats from an IEntityStats
        /// </summary>
        /// <param name="stats">stats to copy</param>
        /// <returns></returns>
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