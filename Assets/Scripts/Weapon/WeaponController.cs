using System.ComponentModel;
using Core.Item;
using EasyButtons;
using Item;
using PlayerScripts;
using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(ItemPrefabController)), RequireComponent(typeof(Animator))]
    public class WeaponController : MonoBehaviour
    {
        private Animator animator;
        private ItemPrefabController gateway;

        public WeaponFollowConfig followConfig;

        [Tooltip("x offset from player when attacking")]
        public float attack_offset = 0.5f;

        [Tooltip("Rotation of the weapon when idle")]
        public float RotationWhenIdle = 90f;

        [Tooltip("Number of attack types for this weapon")]
        public int AttacksCount = 3;

        [Tooltip("The next attack in within this period will be considered a consecutive attack (resulting in combo) in seconds")]
        public float ComboWaitPeriod_Secs = 0.5f;

        [field: SerializeField, Tooltip("Current combo counter"), ReadOnly(true)]
        public int ComboCounter { get; private set; } = 0;
        
        private float lastAttackTime = 0f;
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int AttackIndex = Animator.StringToHash("AttackIndex");

        public ItemInstance weaponItem => gateway?.slot.Item.itemInstance;

        // Start is called before the first frame update
        void Start()
        {
            gateway = GetComponent<ItemPrefabController>();
            animator = GetComponent<Animator>();
            if (AttacksCount == 0)
            {
                Debug.LogError("No Available Attacks!");
            }

            if (gateway)
            {
                gateway.OnItemUsed += Attack;
                gateway.OnItemReleased += AttackRelease;
            }
        }


        void FixedUpdate()
        {
            if (IsAttacking) // Using if else statement to avoid many tenery operators (?:) checking for IsAttacking
            {
                float offset = -attack_offset;
                transform.position = Vector3.Lerp(transform.position, Player.Transform.position - Player.Transform.right * offset, followConfig.attackTravelSpeed);
                transform.rotation = Player.Transform.rotation;
            }
            else
            {
                float offset = followConfig.followOffset + gateway.slot.slotIndex * followConfig.indexOffset;
                transform.position = Vector3.Lerp(transform.position, Player.Transform.position - Player.Transform.right * offset, followConfig.travelSpeed);
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
            int clipIndex = ComboCounter % (AttacksCount - 1);
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
            weaponItem.item.itemUseSoundEffect?.PlayAtPoint(transform.position);
            ExecuteComboCheck();
        }

        public bool IsAttacking => !animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle");

        /// <summary>
        /// When the player releases the attack button, this function is called. cCn be used to cancel attacks
        /// </summary>
        [Button]
        public void AttackRelease() { }
    }
}