using System;
using System.Linq;
using EasyButtons;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextHyperlink : MonoBehaviour,  IPointerClickHandler
    {

        public enum HyperlinkType
        {
            URL
        }
        [Serializable]
        public struct Hyperlink
        {
            public string id;
            public string uri;
            public HyperlinkType type;
        }

        public Hyperlink[] links = new Hyperlink[0];
        private TextMeshProUGUI text;
        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        [Button]
        public void Test()
        {
            Application.OpenURL("https://www.google.com");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, eventData.pressEventCamera);
            if (linkIndex == -1) return;
            var linkinfo = text.textInfo.linkInfo[linkIndex];
            if (links.Length == 0) return;
            var matches = links.Where(x => x.id == linkinfo.GetLinkID()).ToArray();
            if (matches.Length == 0) return;
            Hyperlink hyperLink = matches.First();
            switch (hyperLink.type)
            {
                case HyperlinkType.URL:
                    Application.OpenURL(hyperLink.uri);
                    return;
                
                default:
                    Debug.Log("Unknown hyperlink type");
                    return;
            }
        }
    }
}
