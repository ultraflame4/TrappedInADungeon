using Core.Save;
using Core.Utils;
using UnityEngine;

namespace UI.SaveList
{
    /// <summary>
    /// Manages the save list in the save menu
    /// </summary>
    public class SaveListManager : MonoBehaviour
    {
        [Tooltip("Prefab for the save list item")]
        public GameObject saveListItemPrefab;
        [Tooltip("The content transform of the scroll view")]
        public Transform saveListContent;
        private void Start()
        {
            // Ensure that the saveListContent is empty
            saveListContent.DestroyChildren();
            // Create a save list item for each game save
            foreach (string saveName in GameSaveManager.GetSaves())
            {
                GameObject saveListItem = Instantiate(saveListItemPrefab, saveListContent);
                // Configure the save list item with the correct save name
                saveListItem.GetComponent<SaveListItemController>().SetSave(saveName);
            }
        }
    }
}