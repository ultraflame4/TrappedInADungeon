using System;
using System.Collections;
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
        public TextMeshProUGUI loadingText;
        public Image logo;
        private Image image;
        private Coroutine currentCoroutine;
        
        private void Start()
        {
            image = GetComponent<Image>();
            loadingText.gameObject.SetActive(false);
            logo.gameObject.SetActive(false);
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
            loadingText.gameObject.SetActive(true);
            logo.gameObject.SetActive(true);
            yield return effectB.FadeToBlackCoroutine();
            loadingText.gameObject.SetActive(false);
            logo.gameObject.SetActive(false);
            effectB.ClearOut();
            yield return effectA.FadeToClearCoroutine();
            image.raycastTarget = false; // Disable so that the player can interact with ui
        }

        IEnumerator FadeInCoroutine()
        {
            image.raycastTarget = true; // Block ui interaction
            yield return effectA.FadeToBlackCoroutine();
            effectB.BlackOut();
            loadingText.gameObject.SetActive(true);
            logo.gameObject.SetActive(true);
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
                loadingText.text = $"Loading - {asyncOperation.progress * 100}%";
                if (asyncOperation.progress >= 0.9f) break;
                yield return null;
            }

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