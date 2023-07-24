﻿using System;
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
        private TextMeshPro text;
        private Rigidbody2D rb;
        public float number;
        public float targetMaxHealth;
        public Color startColor;
        public Color endColor;
        public Action destroyCallback;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            text = GetComponent<TextMeshPro>();
            
            text.text = $"{number.ToPrecision(0)}";
            text.color = Color.Lerp(startColor, endColor, Mathf.Clamp01(number / targetMaxHealth));
            rb.velocity = new Vector2(Random.value, Random.value)* (300 + (Random.value * 100)) * Time.deltaTime;
            StartCoroutine(DeleteSelf());
        }
        
        IEnumerator DeleteSelf()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            destroyCallback?.Invoke();
        }
    }
}