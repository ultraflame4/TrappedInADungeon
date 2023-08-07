namespace Core.Enemies
{
    /// <summary>
    /// The various states an enemy can be in.
    /// </summary>
    public enum EnemyStates
    {
        // Stunned
        STUNNED,

        // Idle
        PATROL,

        // Moving towards player
        ALERT,

        // Attacking player
        ATTACK,

        // Is dead
        DEAD
    }
}