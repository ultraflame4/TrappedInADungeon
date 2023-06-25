using System.ComponentModel;
using EasyButtons;
using UnityEngine;

namespace Weapon
{
    public class WeaponController : MonoBehaviour
    {
        public Animator animator;
        public Transform player;
        public float offset=0.5f;
        public float travelSpeed=0.05f;
        public float RotationWhenIdle = 90f;
        public AnimatorOverrideController overrideController;

        /// <summary>
        /// Hitboxes (colliders) for attacks will be activated and stored inside of animation clips!.
        /// This allows for exact timing of when the colliders are enabled (and then disabled)
        /// </summary>
        public AnimationClip[] attackClips;

        /// <summary>
        /// The amount of time to wait for.
        /// The next attack in while in this period will be considered a consecutive attack (resulting in combo)
        /// In Seconds
        /// </summary>
        public float ComboWaitPeriod_Secs = 0.5f;

        /// <summary>
        /// Number of combos
        /// </summary>
        [ReadOnly(true)] public int ComboCounter = 0;

        private float lastAttackTime = 0f;
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");

        // Start is called before the first frame update
        void Start()
        {
            if (attackClips.Length == 0)
            {
                Debug.LogError("No Available Attack Clips!");
            }
        }

        
        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position,player.transform.position - player.transform.right * offset, travelSpeed);
            transform.rotation = Quaternion.Euler(0, 0, IsAttacking?0:RotationWhenIdle);
        }


        /// <summary>
        /// Checks if the attack is consecutive and therefore results in a combo
        /// </summary>
        private void ExecuteComboCheck()
        {
            float currentAttackTime = Time.time;
            // If time between last attack and now is lesser than wait period, it is a consecutive attack
            bool isConsecutive = (currentAttackTime - lastAttackTime) < ComboWaitPeriod_Secs;
            lastAttackTime = currentAttackTime;
            if (isConsecutive) // If consecutive attack, increase combo counter
            {
                ComboCounter++;
            }
            else // Else reset combo
            {
                ComboCounter = 0;
            }
        }

        /// <summary>
        /// This function swaps out the current attack clip based on the current combo.
        /// </summary>
        private void SwapAttackClip()
        {
            int clipIndex = ComboCounter % attackClips.Length;
            overrideController["BaseWeaponAttack"] = attackClips[clipIndex];
        }


        public bool IsAttacking => !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        /// <summary>
        /// Executes an attack. THIS WILL NOT CANCEL ATTACKS
        /// </summary>
        [Button]
        public void Attack()
        {
            if (IsAttacking)
            {
                return;
            }
            SwapAttackClip();
            animator.SetTrigger(AttackTrigger);
            ExecuteComboCheck();
            
        }
        /// <summary>
        /// When the player releases the attack button, this function is called. cCn be used to cancel attacks
        /// </summary>
        [Button]
        public void AttackRelease()
        {
            
        }
        
        
        
    }
}