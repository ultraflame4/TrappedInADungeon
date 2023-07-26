using Core.Save;
using Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.SaveList
{
    public class StartNewBtn : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI inputText;
        public void Click()
        {
            if (GameSaveManager.SaveExists(inputText.text))
            {
                Debug.LogWarning($"Save {inputText.text} already exists!");
                NotificationManager.Instance.PushNotification("<color=#f00>Error :\nSave already exists!\nDelete the save manually first!</color>");
                return;
            }
            GameManager.Instance.LoadGame(inputText.text);
        }
    }
}