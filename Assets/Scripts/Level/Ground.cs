using System;
using UnityEngine;

namespace Level
{
    public class Ground : MonoBehaviour
    {
        public void UpdateWidth(float amt, int sections)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = amt;
            transform.localScale = localScale;
            Vector3 localPos = transform.localPosition;
            float sectionWidth = amt / sections /2;
            localPos.x = sectionWidth * (sections-1);
            transform.localPosition = localPos;
        }
    }
}