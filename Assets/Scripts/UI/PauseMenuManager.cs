using System;
using UnityEngine;

namespace UI
{
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
            gameObject.SetActive(false);
        }

        public void MainMenuBtn()
        {
            
        }
        public void SettingsBtn()
        {
            
        }
        public void QuitBtn()
        {
            
        }
    }
}