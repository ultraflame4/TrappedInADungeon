using System;
using UnityEngine;

namespace UI.Tabs
{
    public class TabsManager : MonoBehaviour
    {
        public TabController[] tabs;
        public GameObject[] tabContents;
        private int currentTab = 0;
        public int CurrentTabIndex => currentTab;
        /// <summary>
        /// TabChanged event. Parameter is the new current tab;
        /// </summary>
        public event Action<int> TabChanged;

        private void Start()
        {

            if (tabContents.Length != tabs.Length)
            {
                Debug.LogWarning("the number of tabs and the number of tabContents does not match!");
            }
            for (var i = 0; i < tabs.Length; i++)
            {
                var tab = tabs[i];
                tab.Setup(this, i);
            }

            OpenTab(0);
        }

        /// <summary>
        /// Sets the current Tab index to the specified index and activates the corresponding tab content page
        /// </summary>
        /// <param name="tab_index"></param>
        /// <returns>Returns true if successful.</returns>
        public bool OpenTab(int tab_index)
        {
            if (tab_index < 0)
            {
                Debug.LogError("Invalid tab index!");
                return false;
            }
            currentTab = tab_index;
            TabChanged?.Invoke(currentTab);
            for (var i = 0; i < tabContents.Length; i++)
            {
                GameObject obj = tabContents[i];
                obj.SetActive(currentTab==i);
            }
            return true;
        }
        
        
    }
}