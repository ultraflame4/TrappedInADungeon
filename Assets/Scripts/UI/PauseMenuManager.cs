using System;
using UnityEngine;

namespace UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.Instance.GamePaused.Changed += UpdatePauseMenu;
            
        }

        void UpdatePauseMenu()
        {
            gameObject.SetActive(GameManager.Instance.GamePaused.value);
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