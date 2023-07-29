using System;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// The Pause Menu also handles death
    /// </summary>
    public class PauseMenuManager : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.Instance.GamePaused.Changed += UpdatePauseMenu;
            gameObject.SetActive(false);
        }

        void UpdatePauseMenu()
        {
            Debug.Log("Test");
            gameObject.SetActive(GameManager.Instance.GamePaused);
        }

        public void ResumeBtn()
        {
            GameManager.Instance.GamePaused.value = false;
        }

        public void MainMenuBtn()
        {
            GameManager.Instance.QuitToMainMenu();
        }
        public void SettingsBtn()
        {
            
        }
        public void QuitBtn()
        {
            GameManager.Instance.QuitGame();
        }
    }
}