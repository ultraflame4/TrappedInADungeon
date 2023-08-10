using System;
using Core.Sound;
using Core.Utils;
using UnityEngine;

namespace PlayerScripts
{
    /// <summary>
    /// Sound effects for the player
    /// </summary>
    [RequireComponent(typeof(Movement))]
    public class PlayerSoundEffects : MonoBehaviour
    {
        private Movement movement;

        [SerializeField, Tooltip("Walk sound effect")]
        private SoundEffect walk;

        [SerializeField, Tooltip("Jump sound effect")]
        private SoundEffect jump;

        [SerializeField, Tooltip("Land on ground sound effect")]
        private SoundEffect land;

        [SerializeField, Tooltip("Dash sound effect")]
        private SoundEffect dash;


        private void Awake()
        {
            movement = GetComponent<Movement>();
            // Create the audio sources for sound effects
            walk.Create(gameObject);
            jump.Create(gameObject);
            land.Create(gameObject);
            dash.Create(gameObject);

            // Register the movement events
            movement.JumpEvent += OnJump;
            movement.LandEvent += OnLand;
            movement.DashEvent += OnDash;
        }

        // ---- The callbacks which will play the corresponding audio sources ----
        void OnDash() => dash.audioSrc.Play();

        void OnJump() => jump.audioSrc.Play();

        void OnLand() => land.audioSrc.Play();
        // ---- end ----

        private void Update()
        {
            // If player is walking, play the walk sfx (if it isn't playing already) and set it to loop
            if (movement.IsWalking)
            {
                if (!walk.audioSrc.isPlaying)
                {
                    walk.audioSrc.Play();
                    walk.audioSrc.loop = true;
                }
            }
            else // When player stops walking, turn off looping so it doesnt play the walk sfx again
            {
                walk.audioSrc.loop = false;
            }
        }
    }
}