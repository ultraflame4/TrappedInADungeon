using System;
using System.Collections.Generic;
using Item;
using UI.Dragging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class CursorController : MonoBehaviour
    {
        public Image image;
        private static CursorController instance;
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("WARNING! Multiple CursorControllers in scene! Current static instance will be replaced!");
            }

            instance = this;
        }

        private void Start()
        {
            image.SetSprite(null);
        }

        public static CursorController GetInstance()
        {
            if (instance == null)
            {
                Debug.LogError("WARNING! No CursorController in scene!");
            }

            return instance;
        }

        /// <summary>
        /// Set the cursor sprite
        /// </summary>
        /// <param name="sprite"></param>
        public void SetSprite(Sprite sprite = null)
        {
            image.SetSprite(sprite);
        }

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, Input.mousePosition, 0.5f);
        }
    }
}