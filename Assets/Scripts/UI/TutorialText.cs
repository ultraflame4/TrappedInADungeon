using UnityEngine;

namespace UI
{
    public class TutorialText : MonoBehaviour
    {
        [Header("Automatically disables tutorial text if not in first area")]
        private string _; // This is a hack to make the header show up in the inspector

        void Start()
        {
            // Disable tutorial text if not in first area
            if (GameManager.CurrentAreaIndex != 0) gameObject.SetActive(false);
        }
    }
}