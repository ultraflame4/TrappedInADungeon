using System;
using UnityEngine;

namespace UI
{
    public class TabsManager : MonoBehaviour
    {
        public TabController[] tabs;
        public GameObject[] tabContents;
        public int CurrentTabIndex { get; private set; }

        private void Start()
        {
            if (tabContents.Length != tabs.Length) Debug.LogWarning("the number of tabs and the number of tabContents does not match!");
            for (var i = 0; i < tabs.Length; i++)
            {
                var tab = tabs[i];
                tab.Setup(this, i);
            }

            OpenTab(0);
        }

        /// <summary>
        /// TabChanged event. Parameter is the new current tab;
        /// </summary>
        public event Action<int> TabChanged;

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

            CurrentTabIndex = tab_index;
            TabChanged?.Invoke(CurrentTabIndex);
            for (var i = 0; i < tabContents.Length; i++)
            {
                var obj = tabContents[i];
                obj.SetActive(CurrentTabIndex == i);
            }

            return true;
        }
    }
}