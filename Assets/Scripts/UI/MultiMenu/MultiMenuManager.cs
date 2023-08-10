using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using UnityEngine;

namespace UI.MultiMenu
{
    /// <summary>
    /// Manages the various menus in the game.
    /// </summary>
    public class MultiMenuManager : MonoBehaviour
    {
        [Tooltip("The first menu, such as the main menu.")]
        public GameObject rootMenu;
        [SerializeField]
        private List<GameObject> history = new(); // The history of menus opened. The last item is the current menu.

        private void Awake()
        {
            // Add the root menu to the history
            history.Add(rootMenu);
        }

        /// <summary>
        /// Push a new menu to the history
        /// </summary>
        /// <param name="newMenu"></param>
        public void PushHistory(GameObject newMenu)
        {
            history.Add(newMenu);
        }
        
        /// <summary>
        /// Return to the previous menu in the history (unless there is only one menu in the history which is the root menu
        /// </summary>
        public void BackMenu()
        {
            // If there is only one menu in the history, then we are at the root menu and cannot go back
            if (history.Count <= 1) return;
            // Close the current menu
            history.Last().SetActive(false);
            // Remove the current menu from the history
            history.RemoveAt(history.Count - 1);
            // Open the previous menu which is now at the end of the history
            history.Last().SetActive(true);
        }
    }
}