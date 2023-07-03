using System;
using System.Collections;
using UnityEngine;

namespace Entities
{
    public class DeadShadow : MonoBehaviour
    {
        public float durationMS = 1000f;

        private void Start()
        {
            StartCoroutine(StartFade());
        }
        
        IEnumerator StartFade()
        {
            yield return new WaitForSeconds(durationMS / 1000f);
            Destroy(gameObject);
        }
    }
}