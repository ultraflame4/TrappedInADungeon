using EasyButtons;
using UnityEngine;

namespace Weapon
{
    public class WeaponController : MonoBehaviour
    {
        public Animator animator;

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
        public float ComboWaitPeriod_Secs= 0.5f;
        /// <summary>
        /// Number of combos
        /// </summary>
        [HideInInspector]
        public int ComboCounter = 0; 
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        [Button]
        public void Attack()
        {
            
        }
    }
}
