using System;
using System.Linq;
using EasyButtons;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextHyperlink : MonoBehaviour, IPointerClickHandler
    {
        public enum HyperlinkType
        {
            URL // Open the link in the browser
        }

        [Serializable]
        public struct Hyperlink
        {
            [Tooltip("The id of the hyperlink. Corresponds to the link id in the text element eg. <link=id>")]
            public string id;

            [Tooltip("The uri to open when the link is clicked")]
            public string uri;

            [Tooltip("The type of hyperlink")]
            public HyperlinkType type;
        }

        [Tooltip("The various hyperlinks in the text element")]
        public Hyperlink[] links = new Hyperlink[0];

        // component reference :D
        private TextMeshProUGUI text;

        private void Awake()
        {
            // Get the text component
            text = GetComponent<TextMeshProUGUI>();
        }

        // This is called when this ui element is clicked :D
        public void OnPointerClick(PointerEventData eventData)
        {
            // Find the link that was clicked
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, eventData.pressEventCamera);
            if (linkIndex == -1) return; // No link was clicked therefore skip
            // Get the link info from link index
            var linkinfo = text.textInfo.linkInfo[linkIndex];
            if (links.Length == 0) return; // No links were defined therefore skip
            // Find the hyperlink that matches the link id
            var matches = links.Where(x => x.id == linkinfo.GetLinkID()).ToArray();
            if (matches.Length == 0) return; // No hyperlinks matched the link id therefore skip
            // Get the first match
            Hyperlink hyperLink = matches.First();
            // Switch on the hyperlink type
            switch (hyperLink.type)
            {
                case HyperlinkType.URL: // Open the link in the browser
                    Application.OpenURL(hyperLink.uri);
                    return;

                default:
                    Debug.Log("Unknown hyperlink type");
                    return;
            }
        }
    }
}