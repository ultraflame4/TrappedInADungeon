using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public float sceneLoadProgress { get; private set; }

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
            counter = transitionSteps;
            while (counter>0)
            {
                image.material.SetFloat(Threshold, counter/(float)transitionSteps);
                counter--;
                yield return new WaitForSeconds(transitionTime / transitionSteps);
            }
        }
        IEnumerator FadeToClearCoroutine()
        {
            counter = 0;
            while (counter<transitionSteps)
            {
                image.material.SetFloat(Threshold, counter/(float)transitionSteps);
                counter++;
                yield return new WaitForSeconds(transitionTime / transitionSteps);
            }
        }
        

        public void FadeToClear()
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);

            currentCoroutine=StartCoroutine(FadeToClearCoroutine());
        }
        
        public IEnumerator TransitionToSceneCoroutine(string sceneName)
        {
            yield return null;
            yield return FadeToBlackCoroutine();
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            
            while (!asyncOperation.isDone)
            {
                sceneLoadProgress = asyncOperation.progress;
                yield return null;
            }
            yield return null;
        }
        /// <summary>
        /// Loads & transitions to a scene with the given name.
        /// </summary>
        /// <param name="sceneName"></param>
        public void TransitionToScene(string sceneName)
        {
            StartCoroutine(TransitionToSceneCoroutine(sceneName));
        }
    }
}