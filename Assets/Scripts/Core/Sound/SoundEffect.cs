using System;
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
             audioSrc = gameObject.AddComponent<AudioSource>();
             audioSrc.clip = clip;
             audioSrc.loop = loop;
             audioSrc.volume = volume;
             return audioSrc;
        }

        /// <summary>
        /// Plays this at a point in the world position. <br/>
        /// This is essentially a wrapper for <see cref="AudioSource.PlayClipAtPoint(UnityEngine.AudioClip,UnityEngine.Vector3)">AudioSource.PlayClipAtPoint</see>
        /// </summary>
        public void PlayAtPoint(Vector3 worldPosition)
        {
            AudioSource.PlayClipAtPoint(clip,worldPosition,volume);
        }
    }
}