using UnityEngine;

namespace PlayerScripts
{
    /// <summary>
    /// Current purpose is to provide a centralised way to access player related stuff.
    /// </summary>
    [RequireComponent(typeof(PlayerBody)), RequireComponent(typeof(PlayerInventory))]
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// Reference to the player inventory
        /// </summary>
        public PlayerInventory inventory { get; private set; }
        /// <summary>
        /// Reference to the player body
        /// </summary>
        public PlayerBody body { get; private set; }
        /// <summary>
        /// The singleton static instance
        /// </summary>
        public static Player Instance { get; private set; }
        /// <summary>
        /// Shorthand for PlayerController.Instance.<see cref="body"/>
        /// </summary>
        public static PlayerBody Body => Instance.body;
        /// <summary>
        /// Shorthand for PlayerController.Instance.<see cref="inventory"/>
        /// </summary>
        public static PlayerInventory Inventory => Instance.inventory;
        /// <summary>
        /// Shorthand for PlayerController.Instance.<see cref="transform"/>
        /// </summary>
        public static Transform Transform => Instance.transform;


        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning($"Multiple instance of PlayerController found in Scene! The current static instance will be replaced by this one. This is probably not what you want! (Or good)");
            }

            Instance = this;
            // Get references
            body = GetComponent<PlayerBody>();
            inventory = GetComponent<PlayerInventory>();
        }

    }
}