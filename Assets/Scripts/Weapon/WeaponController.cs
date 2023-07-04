using System.ComponentModel;
using EasyButtons;
using Item;
using UnityEngine;

namespace Weapon
{
    public class WeaponController : MonoBehaviour
    {
        public Animator animator;
        public WeaponFollowConfig followConfig;
        public ItemPrefabHotbarGateway gateway;
        public float attack_offset=0.5f;
        public float RotationWhenIdle = 90f;
        public int AttacksCount = 3; // Number of attacks for this weapon
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

        public Transform player;

        private float lastAttackTime = 0f;
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int AttackIndex = Animator.StringToHash("AttackIndex");

        public WeaponItem weaponItem => gateway?.slot.Item.itemInstance as WeaponItem;
        // Start is called before the first frame update
        void Start()
        {
            if (AttacksCount == 0)
            {
                Debug.LogError("No Available Attacks!");
            }

            if (gateway)
            {
                gateway.OnItemUsed += Attack;
                gateway.OnItemReleased += AttackRelease;   
            }
            player = GameObject.FindWithTag("Player").transform;
        }

        
        void FixedUpdate()
        {
            
            if (IsAttacking) // Using if else statement to avoid many tenery operators (?:) checking for IsAttacking
            {
                float offset = -attack_offset;
                transform.position = Vector3.Lerp(transform.position,player.transform.position - player.transform.right * offset, followConfig.attackTravelSpeed);
                transform.rotation = player.transform.rotation;
            }
            else
            {
                float offset =  followConfig.followOffset+gateway.slot.slotIndex*followConfig.indexOffset;
                transform.position = Vector3.Lerp(transform.position,player.transform.position - player.transform.right * offset, followConfig.travelSpeed);
                transform.rotation = Quaternion.Euler(0, 0, RotationWhenIdle);
            }
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
            int clipIndex = ComboCounter % (AttacksCount-1);
            animator.SetInteger(AttackIndex, clipIndex);
            // overrideController["BaseWeaponAttack"] = attackClips[clipIndex];
        }


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

        public bool IsAttacking => !animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle");

        /// <summary>
        /// When the player releases the attack button, this function is called. cCn be used to cancel attacks
        /// </summary>
        [Button]
        public void AttackRelease()
        {
            
        }
        
        
        
    }
}