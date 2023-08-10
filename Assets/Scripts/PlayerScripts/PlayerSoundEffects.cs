using System;
using Core.Sound;
using Core.Utils;
using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(Movement))]
    public class PlayerSoundEffects : MonoBehaviour
    {
        private Movement movement;

        [SerializeField]
        private SoundEffect walk;

        [SerializeField]
        private SoundEffect jump;

        [SerializeField]
        private SoundEffect land;

        [SerializeField]
        private SoundEffect dash;
        

        private void Awake()
        {
            movement = GetComponent<Movement>();
            walk.Create(gameObject);
            jump.Create(gameObject);
            land.Create(gameObject);
            dash.Create(gameObject);
            
            movement.JumpEvent += OnJump;
            movement.LandEvent += OnLand;
            movement.DashEvent += OnDash;
        }

        void OnDash() => dash.audioSrc.Play();

        void OnJump() => jump.audioSrc.Play();

        void OnLand() => land.audioSrc.Play();

        private void Update()
        {
            if (movement.IsWalking)
            {
                if (!walk.audioSrc.isPlaying)
                {
                    walk.audioSrc.Play();
                    walk.audioSrc.loop = true;
                }
            }
            else
            {
                walk.audioSrc.loop = false; // Dont stop audio jst let it play out
            }
        }
    }
}