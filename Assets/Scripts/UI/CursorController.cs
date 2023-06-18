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
        private DragEventData draggedData = null;
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
        public void SetDragEventData(DragEventData data=null)
        {
            draggedData = data;
            image.SetSprite(data?.GetSprite());
            
        }
        /// <summary>
        /// Get the drag event data from the cursor.
        /// </summary>
        /// <typeparam name="T">Specifies the DragEventData Type. If current DragEventData type does not match, returns null</typeparam>
        public T GetDragEventData<T>() where T : DragEventData 
        {
            return draggedData as T;
        }
        
        

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position,Input.mousePosition,0.5f);
        }
        
    }
}