using UnityEngine;

namespace UI
{
    public class TutorialText : MonoBehaviour
    {
        [Header("Automatically disables tutorial text if not in first area")]
        private string _;
        
        void Start()
        {
            if (GameManager.CurrentAreaIndex != 0)
            {
                gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
