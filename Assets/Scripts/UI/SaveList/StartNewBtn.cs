using TMPro;
using UnityEngine;

namespace UI.SaveList
{
    public class StartNewBtn : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;
        public void Click()
        {
            GameManager.Instance.LoadGame(text.text);
        }
    }
}