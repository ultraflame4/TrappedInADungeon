using System;
using UnityEngine;

namespace Level
{
    public class Ground : MonoBehaviour
    {
        public void UpdateWidth(float amt)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = amt;
            transform.localScale = localScale;

        }
    }
}