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
            image.material = new Material(image.material);
        }

        public void BlackOut()
        {
            image.material.SetFloat(ShaderPropIdThreshold, 0);
        }

        public void ClearOut()
        {
            image.material.SetFloat(ShaderPropIdThreshold, 1);
        }


        public IEnumerator FadeToBlackCoroutine()
        {
            counter = transitionSteps;
            while (counter > 0)
            {
                image.material.SetFloat(ShaderPropIdThreshold, counter / (float)transitionSteps);
                counter--;
                yield return new WaitForSecondsRealtime(transitionTime / transitionSteps);
            }
        }

        public IEnumerator FadeToClearCoroutine()
        {
            Debug.Log("Fading,,");
            counter = 0;
            while (counter < transitionSteps)
            {
                image.material.SetFloat(ShaderPropIdThreshold, counter / (float)transitionSteps);
                counter++;
                yield return new WaitForSecondsRealtime(transitionTime / transitionSteps);
            }
        }
    }
}