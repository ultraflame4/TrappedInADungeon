using TMPro;
using UnityEngine;

namespace UI.SaveList
{
    public class SaveListItemController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;
        private string saveName;

        public void SetSave(string saveName)
        {
            this.saveName = saveName;
            text.text = saveName;
        }

        public void ItemClicked()
        {
            Debug.Log($"Opening save {saveName}");
            GameManager.Instance.LoadGame(saveName);
        }
    }
}