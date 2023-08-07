using UnityEngine;

namespace Core.UI
{
    /// <summary>
    /// Helper component for displaying a bar with inner and outer component.
    /// </summary>
    public class BarController : MonoBehaviour
    {
        [Tooltip("How much of the bar is filled."), Range(0, 1)]
        public float filledPercentage;

        [Tooltip("The transform of the inner bar.")]
        public RectTransform barInner;

        [Tooltip("The transform of the outer bar.")]
        public RectTransform barOuter;

        [Tooltip("Whether the bar is vertical or not.")]
        public bool isVertical;

        public void UpdateBar()
        {
            if (isVertical)
            {
                // Change inner bar width to match the outer bar width
                // Change inner bar height to match the percentage filled
                barInner.sizeDelta = new Vector2(barOuter.sizeDelta.x, filledPercentage * barInner.sizeDelta.y);
                // Move the inner bar to the bottom of the outer bar
                barInner.localPosition = new Vector3(0, barInner.rect.height / 2 - barOuter.rect.height / 2);
                return;
            }

            // Change inner bar width to match the outer bar width
            // Change inner bar height to match the percentage filled
            barInner.sizeDelta = new Vector2(filledPercentage * barOuter.sizeDelta.x, barInner.sizeDelta.y);
            // Move the inner bar to the left of the outer bar
            barInner.localPosition = new Vector3(barInner.rect.width / 2 - barOuter.rect.width / 2, 0);
        }

        private void OnValidate()
        {
            // Update the bar when the values are changed in the inspector
            UpdateBar();
        }
    }
}