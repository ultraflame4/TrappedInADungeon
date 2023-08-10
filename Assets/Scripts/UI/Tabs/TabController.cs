using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Tabs
{
    [RequireComponent(typeof(Image))]
    public class TabController : MonoBehaviour, IPointerClickHandler
    {

        [Tooltip("Sprite to use when tab is inactive")]
        public Sprite InactiveSprite;
        [Tooltip("Sprite to use when tab is active")]
        public Sprite ActiveSprite;
        [Tooltip("Image to display the sprite on")]
        public Image image;
        public TextMeshProUGUI text;
        private TabsManager Manager = null;
        /// <summary>
        /// The index of the tab in the TabsManager.
        /// 
        /// Negative when not set
        /// </summary>
        private int tab_index = -1; 

        /// <summary>
        /// Configure this tab to be managed by the given manager
        /// </summary>
        /// <param name="manager">Tab manager</param>
        /// <param name="index">The index of this tab</param>
        public void Setup(TabsManager manager, int index)
        {
            tab_index = index;
            Manager = manager;
            // Listen to tab changes
            Manager.TabChanged += OnTabChange;
        }

        private void OnTabChange(int newCurrentTabIndex)
        {
            // Set active if this tab is the current tab
            SetActive(newCurrentTabIndex == tab_index);
        }

        
        public void SetActive(bool active = true)
        {
            if (active) // if set active, use the active sprite and black text
            {
                image.sprite = ActiveSprite;
                text.color = Color.black;
                return;
            }
            // if set inactive, use the inactive sprite and white text
            image.sprite = InactiveSprite;
            text.color = Color.white;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // If not managed by a tab manager, throw an error
            if (Manager is null)
            {
                Debug.LogError("This tab is not added to a tab manager!");
                return;
            }
            // If this tab was clicked, tell the manager to open this tab
            Manager.OpenTab(tab_index);
        }
    }
}