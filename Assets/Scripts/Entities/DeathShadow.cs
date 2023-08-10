using System;
using System.Collections;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DeathShadow : MonoBehaviour
    {
        [Tooltip("Duration of the fade in milliseconds")]
        public float durationMS = 1000f;
        [Tooltip("Whether to destroy the object after shadow has faded")]
        public bool autoDestroy = true;
        private SpriteRenderer spriteRenderer;
        private static readonly int PropThreshold = Shader.PropertyToID("_Threshold");

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutine(StartFade());
        }
        
        IEnumerator StartFade()
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 100; i++)
            {
                spriteRenderer.material.SetFloat(PropThreshold, i/100f);
                yield return new WaitForSeconds(durationMS/1000/100); // Divide by 1000 to convert to seconds, then divide by 100 duration for each step
            }
            if (autoDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}