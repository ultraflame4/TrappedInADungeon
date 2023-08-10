using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SceneTransition
{
    [RequireComponent(typeof(Image))]
    public class TransitionEffect : MonoBehaviour
    {
        
        [Tooltip("Time in seconds to fade to black / fade to clear")]
        public float transitionTime = 0.5f;
        [Tooltip("How many steps to take to fade to black / fade to clear")]
        public int transitionSteps = 1000;
        private int counter = 0;
        private static readonly int ShaderPropIdThreshold = Shader.PropertyToID("_Threshold");
        private Image image;

        void Start()
        {
            image = GetComponent<Image>();
            image.material = new Material(image.material); // Duplicate the material such that we don't change the material for all images using it
        }

        /// <summary>
        /// Sets the image to black
        /// </summary>
        public void BlackOut()
        {
            image.material.SetFloat(ShaderPropIdThreshold, 0);
        }

        /// <summary>
        /// Sets the image to clear
        /// </summary>
        public void ClearOut()
        {
            image.material.SetFloat(ShaderPropIdThreshold, 1);
        }

        /// <summary>
        /// Fades the image from clear to black
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeToBlackCoroutine()
        {
            counter = transitionSteps;
            while (counter > 0)
            {
                // Decrease threshold for disintegration shader so that less of the image is visible
                image.material.SetFloat(ShaderPropIdThreshold, counter / (float)transitionSteps);
                counter--;
                yield return new WaitForSecondsRealtime(transitionTime / transitionSteps);
            }
        }

        
        /// <summary>
        /// Fades the image from black to clear
        /// </summary>
        /// <returns></returns>
        public IEnumerator FadeToClearCoroutine()
        {
            counter = 0;
            while (counter < transitionSteps)
            {
                // Increase threshold for disintegration shader so that more of the image is visible
                image.material.SetFloat(ShaderPropIdThreshold, counter / (float)transitionSteps);
                counter++;
                yield return new WaitForSecondsRealtime(transitionTime / transitionSteps);
            }
        }
    }
}