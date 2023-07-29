namespace Core.Enemies
{
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