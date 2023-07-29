﻿using System;
using UnityEngine;

namespace Core.Sound
{
    // Helper class to easily deal with clips and audio source components without having a million components on the object
    [Serializable]
    public class SoundEffect
    {
        [SerializeField]
        private AudioClip clip;
        [SerializeField, Range(0,1)]
        private float volume; // initial
        public AudioSource audioSrc { get; private set; }

        /// <summary>
        /// Initialises the sound effect
        /// </summary>
        /// <param name="gameObject">The game object that has this object</param>
        public void Init(GameObject gameObject)
        {
             audioSrc = gameObject.AddComponent<AudioSource>();
             audioSrc.clip = clip;
             audioSrc.loop = false;
             audioSrc.volume = volume;
        }
        
    }
}