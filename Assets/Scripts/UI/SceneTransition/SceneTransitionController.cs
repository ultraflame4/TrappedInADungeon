using System;
using System.Collections;
using Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.SceneTransition
{
    [RequireComponent(typeof(Image))]
    public class SceneTransitionController : MonoBehaviour
    {
        public TransitionEffect effectA;
        public TransitionEffect effectB;
        public GameObject contents;
        public VolatileValue<float> loadProgress { get; private set; } = new();
        private Image image;
        private Coroutine currentCoroutine;
        
        private void Start()
        {
            image = GetComponent<Image>();
            contents.SetActive(false);
            effectA.BlackOut();
            effectB.BlackOut();
        }

        /// <summary>
        /// Starts a new coroutine and ensures that only one coroutine is running at a time
        /// </summary>
        /// <param name="newCurrent"></param>
        /// <returns></returns>
        private Coroutine StartSingle(IEnumerator newCurrent)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(newCurrent);;
            return currentCoroutine;
        }

        
        IEnumerator FadeOutCoroutine()
        {
            contents.SetActive(true);
            yield return effectB.FadeToBlackCoroutine();
            contents.SetActive(false);
            effectB.ClearOut();
            yield return effectA.FadeToClearCoroutine();
            image.raycastTarget = false; // Disable so that the player can interact with ui
            
        }
        IEnumerator FadeInCoroutine()
        {
            image.raycastTarget = true; // Block ui interaction
            yield return effectA.FadeToBlackCoroutine();
            effectB.BlackOut();
            contents.SetActive(true);
            yield return effectB.FadeToClearCoroutine();
        }

        public void FadeOut()
        {
            StartSingle(FadeOutCoroutine());
        }

        public IEnumerator TransitionToSceneCoroutine(string sceneName)
        {
            yield return null;
            yield return FadeInCoroutine();
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                loadProgress.value = asyncOperation.progress;
                if (asyncOperation.progress >= 0.9f) break;
                yield return null;
            }
            loadProgress.value = 1f;
            yield return new WaitForSecondsRealtime(0.5f);
            asyncOperation.allowSceneActivation = true;
            yield return null;
        }

        /// <summary>
        /// Loads & transitions to a scene with the given name.
        /// </summary>
        /// <param name="sceneName"></param>
        public void TransitionToScene(string sceneName)
        {
            StartSingle(TransitionToSceneCoroutine(sceneName));
        }
    }
}