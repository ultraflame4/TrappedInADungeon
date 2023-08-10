using Core.Save;
using Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.SaveList
{
    /// <summary>
    /// Script for the create new save button in the save menu
    /// </summary>
    public class StartNewBtn : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI inputText;
        // Button click event
        public void Click()
        {
            // Check if save already exists, if so, kindly refuse the user
            if (GameSaveManager.SaveExists(inputText.text))
            {
                Debug.LogWarning($"Save {inputText.text} already exists!");
                NotificationManager.Instance.PushNotification("<color=#f00>Error :\nSave already exists!\nDelete the save manually first!</color>");
                return;
            }
            // Load the game with the new save name
            GameManager.Instance.LoadGame(inputText.text);
        }
    }
}