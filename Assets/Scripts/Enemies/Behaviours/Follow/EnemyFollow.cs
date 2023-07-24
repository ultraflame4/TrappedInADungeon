using Core.Enemies;
using Core.Entities;
using UnityEngine;

namespace Enemies.Behaviours.Follow
{
    public class EnemyFollow : EnemyStateBehaviour
    {
        // references
        private EntityBody body;
        private Rigidbody2D rb;
        private Transform player;
        
        [Tooltip("Should the enemy be allowed to fly?")]
        public bool allowFlight;
        [Tooltip("How far will the enemy follow the player before stopping")]
        public float followRange = 5f;

        [Tooltip("Distance to the player before the enemy stops ( and presumably starts attacking )")]
        public float stopDist = 0.1f;
        /// <summary>
        ///  A buffer to make sure the player is well within the stop distance
        /// </summary>
        const float stopDistBuffer = 0.5f;
        [Tooltip("Should the enemy be facing the player when attacking?")]
        public bool checkDirection = true;

        public Vector3 directionToPlayer { get; private set; }
        public Vector3 directionToPlayerSnapped{ get; private set; }
        /// <summary>
        /// Whether the attack animation is playing
        /// </summary>
        private bool attackAnimPlaying => stateManager.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        private void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
            body = GetComponent<EntityBody>();
            rb = GetComponent<Rigidbody2D>();
            UpdateDirections();
            RotateTowardsPlayer();
        }

        /// <summary>
        /// Updates the directionToPlayer and directionToPlayerSnapped variables which are also used by other states behaviors 
        /// </summary>
        void UpdateDirections()
        {
            // Direction to player
            directionToPlayer = (player.position - transform.position).normalized;
            // Direction to player but snapped to the x axis. (y=0)
            // Using boolean logic to determine whether to snap to 1 or -1 else when x=0, the Vector normalisation will freak out between 1 and -1
            directionToPlayerSnapped = new Vector3(directionToPlayer.x > 0 ? 1 : -1, 0);
        }

        private void FixedUpdate()
        {
            UpdateDirections();

            if (!stateActive) return;
            
            // If enemy is not grounded and can't fly, skip moving
            if (!stateManager.isGrounded && !allowFlight) return;

            // If player is out of range, go back to patrol
            if (Vector3.Distance(transform.position, player.position) > followRange)
            {
                rb.velocity = Vector2.zero; // Stop moving when player out of range
                stateManager.TransitionState(EnemyStates.PATROL);
                return;
            }

            RotateTowardsPlayer();
            if (CheckPlayerWithinAttackRange())
            {
                rb.velocity = new Vector2(0, allowFlight ? 0 : rb.velocity.y); // Stop immediately when within range
                return;
            }

            Vector3 vel = (allowFlight ? directionToPlayer : directionToPlayerSnapped) * body.Speed;
            // Get distance to player and subtract attack distance
            float distance = Vector3.Distance(transform.position, player.position) - stopDist + stopDistBuffer;
            // Clamp distance to 0-1. distFactor makes velocity lower the closer the enemy is to the player
            float distFactor = Mathf.Clamp(Mathf.Pow(2 * distance, 2), 0, 1);
            // Scale velocity by distFactor
            // distance will become negative when player is within stopDist. However because distance gets squared, it will always be positive,
            // and we dont want the enemy to move backwards when the player is within stopDist.
            // Hence we simply do a check to see if distance is negative and make distFactor 0 if it is
            vel *= distance > 0 ? distFactor : 0;

            if (!allowFlight) // Preserve y velocity if enemy can't fly
            {
                vel.y = rb.velocity.y;
            }

            if (attackAnimPlaying) return; // Don't move if attack animation is still playing
            rb.velocity = vel * Time.deltaTime;
        }

        public bool CheckPlayerWithinAttackRange()
        {
            if (Vector3.Distance(transform.position, player.position) <= stopDist)
            {
                // If the enemy should be facing the player when attacking, check if the player is within 90 degrees of the enemy's right
                if (checkDirection && Vector2.Angle(directionToPlayer, transform.right) >= 90) return false;
                stateManager.TransitionState(EnemyStates.ATTACK);
                return true;
            }
            if (!allowFlight && Mathf.Abs(player.position.x - transform.position.x) <= stopDist)
            {
                stateManager.TransitionState(EnemyStates.ATTACK);
                return true;
            }

            return false;
        }

        public void RotateTowardsPlayer()
        {
            // Rotate towards player
            // snaps direction to x axis
            transform.right = directionToPlayerSnapped;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, stopDist);
            Gizmos.DrawWireSphere(transform.position, stopDist-stopDistBuffer);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, followRange);
        }
    }
}