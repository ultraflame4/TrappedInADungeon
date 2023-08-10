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
        // Component references
        [SerializeField, Tooltip("The text that displays the title of the pause menu")]
        private TextMeshProUGUI titleText;
        [SerializeField, Tooltip("The text that displays the resume button text")]
        private TextMeshProUGUI resumeText;

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
            // Update the text when the menu is enabled (whenever the game is paused)
            if (titleText != null)
            {
                titleText.text = Player.Body.IsDead ? "You Died" : "Game Paused";
            }

            if (resumeText != null)
            {
                resumeText.text = Player.Body.IsDead ? "Respawn" : "Resume";
            }
        }

        public void ResumeBtn()
        {
            // If the player is dead, respawn them instead of resuming
            if (Player.Body.IsDead)
            {
                GameManager.Instance.RespawnPlayer();
                return;
            }
            // Otherwise, resume the game
            GameManager.Instance.GamePaused.value = false;
        }

        public void MainMenuBtn()
        {
            GameManager.Instance.QuitToMainMenu();
        }

        // Settings button is not implemented yet (and probably won't be)
        public void SettingsBtn() { }

        public void QuitBtn()
        {
            GameManager.Instance.QuitGame();
        }
    }
}