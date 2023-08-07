using System;
using PlayerScripts;
using UnityEngine;

namespace Core.Sound
{
    // Helper class to easily deal with clips and audio source components without having a million components on the object
    [Serializable]
    public class SoundEffect
    {
        
        [SerializeField, Tooltip("The clip for the audio source component")]
        private AudioClip clip;
        [SerializeField, Range(0,1), Tooltip("The volume to initialise the audio source component with.")]
        private float volume; // initial

        /// <summary>
        /// The latest audio source created for this sound effect.
        /// </summary>
        public AudioSource audioSrc { get; private set; }

        /// <summary>
        /// Initialises the sound effect & adds the audio source component to the game object
        /// </summary>
        /// <param name="gameObject">The game object that has this object</param>
        /// <returns>Audio source created</returns>
        public AudioSource Create(GameObject gameObject,bool loop = false)
        {
            // Add audio source component
             audioSrc = gameObject.AddComponent<AudioSource>();
             audioSrc.clip = clip; // Set clip
             audioSrc.loop = loop; // Set loop
             audioSrc.volume = volume; // Set volume
             return audioSrc;
        }

        /// <summary>
        /// Plays this sound effect at a point in the world position. <br/>
        /// This is essentially a wrapper for <see cref="AudioSource.PlayClipAtPoint(UnityEngine.AudioClip,UnityEngine.Vector3)">AudioSource.PlayClipAtPoint</see>
        /// </summary>
        public void PlayAtPoint(Vector3 worldPosition)
        {
            if (Player.Body.IsDead) return; // Don't play sound if player is dead
            if (clip == null) return; // Don't play sound if clip is null
            var obj = new GameObject("SoundEffect PlayAtPoint"); // Create new game object
            var audio = Create(obj,false); // Add audio source component stuff to the game object
            obj.transform.position = worldPosition; // Move game object to world position
            audio.Play(); // Play audio
        }
    }
}