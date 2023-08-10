using System;
using TMPro;
using UnityEngine;

namespace UI.SceneTransition
{
    /// <summary>
    /// Loading progress text :D
    /// </summary>
    public class LoadProgressText : MonoBehaviour
    {
        public SceneTransitionController controller;
        private TextMeshProUGUI text;

        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            // Hook onto the scene transition controller's load progress changed event
            controller.loadProgress.Changed += LoadProgressUpdate;
        }
        
        void LoadProgressUpdate()
        {
            // Whenever the load progress changes, update the text :D
            text.text = $"Loading - {controller.loadProgress * 100}%";
        }

    }
}