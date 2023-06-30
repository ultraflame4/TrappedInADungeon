using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Tabs
{
    [RequireComponent(typeof(Image))]
    public class TabController : MonoBehaviour, IPointerClickHandler
    {

        public Sprite InactiveSprite;
        public Sprite ActiveSprite;
        public Image image;
        public TextMeshProUGUI text;
        private TabsManager Manager = null;
        /// <summary>
        /// The index of the tab in the TabsManager.
        /// 
        /// Negative when not set
        /// </summary>
        private int tab_index = -1; 

        public void Setup(TabsManager manager, int id)
        {
            tab_index = id;
            Manager = manager;
            Manager.TabChanged += OnTabChange;
        }

        private void OnTabChange(int newCurrentTabIndex)
        {
            SetActive(newCurrentTabIndex == tab_index);
        }

        public void SetActive(bool active = true)
        {
            if (active)
            {
                image.sprite = ActiveSprite;
                text.color = Color.black;
                return;
            }
            image.sprite = InactiveSprite;
            text.color = Color.white;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Manager is null)
            {
                Debug.LogError("This tab is not added to a tab manager!");
                return;
            }

            Manager.OpenTab(tab_index);
        }
    }
}