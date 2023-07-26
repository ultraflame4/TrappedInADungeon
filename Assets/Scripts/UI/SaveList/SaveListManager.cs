using Core.Save;
using Core.Utils;
using UnityEngine;

namespace UI.SaveList
{
    public class SaveListManager : MonoBehaviour
    {
        public GameObject saveListItemPrefab;
        public Transform saveListContent;
        private void Start()
        {
            saveListContent.DestroyChildren();
            foreach (string saveName in GameSaveManager.GetSaves())
            {
                GameObject saveListItem = Instantiate(saveListItemPrefab, saveListContent);
                saveListItem.GetComponent<SaveListItemController>().SetSave(saveName);
            }
        }
    }
}