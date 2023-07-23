using UnityEngine;

namespace Core.UI
{
    public class BarController : MonoBehaviour
    {
        [Range(0, 1)] public float filledPercentage;
        public RectTransform barInner;
        public RectTransform barOuter;
        public bool isVertical;
        public void UpdateBar()
        {
            if (isVertical)
            {
                barInner.sizeDelta = new Vector2 ( barOuter.sizeDelta.x,filledPercentage *barInner.sizeDelta.y);
                barInner.localPosition = new Vector3(0, barInner.rect.height/2 - barOuter.rect.height/2);
                return;
            }
            barInner.sizeDelta = new Vector2 (filledPercentage * barOuter.sizeDelta.x,barInner.sizeDelta.y);
            barInner.localPosition = new Vector3(barInner.rect.width/2 - barOuter.rect.width/2, 0);
        }

        private void OnValidate()
        {
            UpdateBar();
        }
    }
}