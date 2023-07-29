using System;
using TMPro;
using UnityEngine;

namespace UI.SceneTransition
{
    public class LoadProgressText : MonoBehaviour
    {
        public SceneTransitionController controller;
        private TextMeshProUGUI text;

        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            controller.loadProgress.Changed += LoadProgressUpdate;
        }
        
        void LoadProgressUpdate()
        {
            text.text = $"Loading - {controller.loadProgress * 100}%";
        }

    }
}