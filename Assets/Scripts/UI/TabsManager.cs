using System;
using UnityEngine;

namespace UI
{
    public class TabsManager : MonoBehaviour
    {
        public TabController[] tabs;
        private int currentTab = 0;
        public int CurrentTabIndex => currentTab;
        /// <summary>
        /// TabChanged event. Parameter is the new current tab;
        /// </summary>
        public event Action<int> TabChanged;

        private void Start()
        {
            for (var i = 0; i < tabs.Length; i++)
            {
                var tab = tabs[i];
                tab.Setup(this, i);
            }
        }

        /// <summary>
        /// Sets the current Tab index to the specified index
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
            return true;
        }
        
        
    }
}