using System;
using System.Collections;
using UnityEngine;

namespace Entities
{
    public class DeathShadow : MonoBehaviour
    {
        public float durationMS = 1000f;
        public SpriteRenderer spriteRenderer;
        private static readonly int Threshold = Shader.PropertyToID("_Threshold");

        private void Start()
        {
            StartCoroutine(StartFade());
        }
        
        IEnumerator StartFade()
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 100; i++)
            {
                spriteRenderer.material.SetFloat(Threshold, i/100f);
                yield return new WaitForSeconds(durationMS/1000/100); // Divide by 1000 to convert to seconds, then divide by 100 duration for each step
            }
            Destroy(gameObject);
        }
    }
}
// todo move this file to another location