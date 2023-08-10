using System;
using System.Collections;
using Core.Utils;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    
    [RequireComponent(typeof(Rigidbody2D)),RequireComponent(typeof(TextMeshPro))]
    public class EntityDamageNumber : MonoBehaviour
    {
        // Component references
        private TextMeshPro text;
        private Rigidbody2D rb;
        
        // Config for the damage number
        [HideInInspector]
        public float number;
        [HideInInspector]
        public float targetMaxHealth;
        
        public Color startColor;
        public Color endColor;
        
        // Callback when this dmgNumber is destroyed
        public Action destroyCallback;

        private void Start()
        {
            // Get components references
            rb = GetComponent<Rigidbody2D>();
            text = GetComponent<TextMeshPro>();
            
            // Set the text to what it was configured for
            text.text = $"{number.ToPrecision(0)}";
            // Set color depending on % of health reduced by damage
            text.color = Color.Lerp(startColor, endColor, Mathf.Clamp01(number / targetMaxHealth));
            // Random upwards velocity to make the damage numbers fly out of enemy
            rb.velocity = new Vector2(Random.value, Random.value)* (300 + (Random.value * 100)) * Time.deltaTime;
            // Start coroutine for de-spawning
            StartCoroutine(DeleteSelf());
        }
        
        IEnumerator DeleteSelf()
        {
            // After 1 sec, de-spawn by destroying itself.
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            // When being destroyed, call the destroyCallback so that in EntityDamagedEffects, the dmgNumberCount is reduced
            destroyCallback?.Invoke();
        }
    }
}