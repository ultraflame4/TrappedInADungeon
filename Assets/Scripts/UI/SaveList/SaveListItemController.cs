using TMPro;
using UnityEngine;

namespace UI.SaveList
{
    /// <summary>
    /// Controller script for the save list item prefab
    /// </summary>
    public class SaveListItemController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;
        private string saveName;

        /// <summary>
        /// Set the save this item is supposed to represent
        /// </summary>
        /// <param name="saveName">Name of save to represent without the .save</param>
        public void SetSave(string saveName)
        {
            this.saveName = saveName;
            text.text = saveName; // Set text to save name
        }

        /// <summary>
        /// Button click event
        /// </summary>
        public void ItemClicked()
        {
            // Load the save this item represents
            Debug.Log($"Opening save {saveName}");
            GameManager.Instance.LoadGame(saveName);
        }
    }
}