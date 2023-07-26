using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using UnityEngine;

namespace UI.MultiMenu
{
    public class MultiMenuManager : MonoBehaviour
    {
        public GameObject rootMenu;
        [SerializeField]
        private List<GameObject> history = new();

        private void Awake()
        {
            history.Add(rootMenu);
        }

        public void PushHistory(GameObject newMenu)
        {
            history.Add(newMenu);
        }
        
        public void BackMenu()
        {
            if (history.Count <= 1) return;
            history.Last().SetActive(false);
            history.RemoveAt(history.Count - 1);
            history.Last().SetActive(true);
        }
    }
}