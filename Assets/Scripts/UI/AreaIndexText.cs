using System;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Shows the index of the current area in the top left corner of the screen
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AreaIndexText : MonoBehaviour
    {
        private TextMeshProUGUI text;

        private void Start()
        {
            // Get text component
            text = GetComponent<TextMeshProUGUI>();
            // Set text to current area index. :D
            text.text = $"Area {GameManager.CurrentAreaIndex}";
        }
    }
}