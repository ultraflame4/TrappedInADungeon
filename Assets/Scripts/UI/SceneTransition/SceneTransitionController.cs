using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SceneTransition
{
    [RequireComponent(typeof(Image))]
    public class SceneTransitionController : MonoBehaviour
    {
        [Tooltip("Time in seconds to fade to black / fade to clear")]
        public float transitionTime = 0.5f;
        public int transitionSteps = 1000;
        private Image image;
        private Coroutine currentCoroutine;
        private int counter = 0;
        private static readonly int Threshold = Shader.PropertyToID("_Threshold");
        void Start()
        {
            image = GetComponent<Image>();
            BlackOut();
        }
        public void BlackOut()
        {
            image.material.SetFloat(Threshold, 1);
        }

        IEnumerator FadeToBlackCoroutine()
        {
            while (counter>0)
            {
                image.material.SetFloat(Threshold, counter/(float)transitionSteps);
                counter--;
                yield return new WaitForSeconds(transitionTime / transitionSteps);
            }
        }
        IEnumerator FadeToClearCoroutine()
        {
            while (counter<transitionSteps)
            {
                image.material.SetFloat(Threshold, counter/(float)transitionSteps);
                counter++;
                yield return new WaitForSeconds(transitionTime / transitionSteps);
            }
        }

        public void FadeToBlack()
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            counter = transitionSteps;
            currentCoroutine=StartCoroutine(FadeToBlackCoroutine());
        }

        public void FadeToClear()
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            counter = 0;
            currentCoroutine=StartCoroutine(FadeToClearCoroutine());
        }
    }
}