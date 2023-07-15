using System;
using UnityEngine;

namespace Level
{
    public class Ground : MonoBehaviour
    {
        public void SetWidth(float amt)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = amt;
            transform.localScale = localScale;

        }
    }
}