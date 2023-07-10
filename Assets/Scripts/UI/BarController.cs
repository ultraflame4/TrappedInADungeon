using System;
using UnityEngine;

namespace UI
{
    public class BarController : MonoBehaviour
    {
        [Range(0, 1)] public float filledPercentage;
        public RectTransform barInner;
        public RectTransform barOuter;
        
        public void UpdateBar()
        {
            barInner.sizeDelta = new Vector2 (filledPercentage * barOuter.sizeDelta.x,barInner.sizeDelta.y);
            barInner.localPosition = new Vector3(barInner.rect.width/2 - barOuter.rect.width/2, 0);
        }

        private void OnValidate()
        {
            UpdateBar();
        }
    }
}