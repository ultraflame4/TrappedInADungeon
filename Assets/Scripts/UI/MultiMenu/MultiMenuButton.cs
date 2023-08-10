using UnityEngine;

namespace UI.MultiMenu
{
    public class MultiMenuButton : MonoBehaviour
    {
        [Tooltip("The manager for the menu")]
        public MultiMenuManager manager;
        [Tooltip("The current menu to close")]
        public GameObject currentMenu;
        [Tooltip("Ui object of the menu to open")]
        public GameObject targetMenu;
        
        public void OpenMenu()
        {
            // When the button is clicked, open the target menu and close the current menu
            targetMenu.SetActive(true);
            currentMenu.SetActive(false);
            // Push the target menu to the history
            manager.PushHistory(targetMenu);
        }
        
    }
}