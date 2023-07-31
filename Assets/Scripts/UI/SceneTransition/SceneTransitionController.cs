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
        [Tooltip("Transition to hide the main scene")]
        public TransitionEffect effectA;
        [Tooltip("Transition to hide the ui elements in transition itself")]
        public TransitionEffect effectB;
        [Tooltip("The ui contents to show during the transition")]
        public GameObject contents;
        public VolatileValue<float> loadProgress { get; private set; } = new();
        private Image image; // Image component. Used to block raycasts
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

        /// <summary>
        /// Fades out of the transition
        /// </summary>
        /// <returns></returns>
        IEnumerator FadeOutCoroutine()
        {
            contents.SetActive(true);
            yield return effectB.FadeToBlackCoroutine();
            contents.SetActive(false);
            effectB.ClearOut();
            yield return effectA.FadeToClearCoroutine();
            image.raycastTarget = false; // Disable so that the player can interact with ui
            
        }
        /// <summary>
        /// Fades into of the transition
        /// </summary>
        /// <returns></returns>
        IEnumerator FadeInCoroutine()
        {
            image.raycastTarget = true; // Block ui interaction
            yield return effectA.FadeToBlackCoroutine();
            effectB.BlackOut();
            contents.SetActive(true);
            yield return effectB.FadeToClearCoroutine();
        }
        
        /// <summary>
        /// Fades out of transition and into the scene
        /// </summary>
        public void FadeOut()
        {
            StartSingle(FadeOutCoroutine());
        }

        /// <summary>
        /// Coroutine to transitions to the scene with the given name (somewhat) smoothly
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public IEnumerator TransitionToSceneCoroutine(string sceneName)
        {
            yield return null; // Skip a frame
            yield return FadeInCoroutine(); // Wait for the fade into transition
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName); // Start asynchronously loading the new scene
            asyncOperation.allowSceneActivation = false; // Don't allow the scene to activate until we say so
            while (!asyncOperation.isDone) // Wait for the scene to load
            {
                // Increase the load progress (so that other components can use it)
                loadProgress.value = asyncOperation.progress; 
                // If the scene is loaded, break out of the loop. Unity stops progress at 0.9f when allowSceneActivation is false
                if (asyncOperation.progress >= 0.9f) break;
                // Wait a frame before looping again
                yield return null;
            }
            // Set load progress to 100%
            loadProgress.value = 1f;
            // Wait a bit before allowing the scene to activate
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