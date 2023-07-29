using System;
using PlayerScripts;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// The Pause Menu also handles death
    /// </summary>
    public class PauseMenuManager : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI titleText { get; private set; }
        [field: SerializeField]
        public TextMeshProUGUI resumeText { get; private set; }

        private void Awake()
        {
            gameObject.SetActive(false);
            GameManager.Instance.GamePaused.Changed += UpdatePauseMenu;
            Player.Body.DeathEvent += UpdatePauseMenu;
        }

        void UpdatePauseMenu()
        {
            gameObject.SetActive(GameManager.Instance.GamePaused || Player.Body.IsDead);
        }

        private void OnEnable()
        {
            if (titleText != null)
            {
                titleText.text = Player.Body.IsDead ? "You Died" : "Game Paused";
            }

            if (resumeText != null)
            {
                titleText.text = Player.Body.IsDead ? "Respawn" : "Resume";
            }
        }

        public void ResumeBtn()
        {
            if (Player.Body.IsDead)
            {
                GameManager.Instance.RespawnPlayer();
                return;
            }
            GameManager.Instance.GamePaused.value = false;
        }

        public void MainMenuBtn()
        {
            GameManager.Instance.QuitToMainMenu();
        }

        public void SettingsBtn() { }

        public void QuitBtn()
        {
            GameManager.Instance.QuitGame();
        }
    }
}