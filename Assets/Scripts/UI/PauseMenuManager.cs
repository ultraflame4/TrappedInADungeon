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
        private void Awake()
        {
            gameObject.SetActive(false);
            GameManager.Instance.GamePaused.Changed += UpdatePauseMenu;
            Player.Body.DeathEvent += UpdatePauseMenu;
        }

        void UpdatePauseMenu()
        {
            gameObject.SetActive(GameManager.Instance.GamePaused || Player.Body.IsDead);
            titleText.text = Player.Body.IsDead ? "You Died" : "Game Paused";
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