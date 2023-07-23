using System;
using Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [RequireComponent(typeof(Image))]
    public class CursorController : MonoBehaviour
    {
        public Image image;
        private static CursorController instance;
        private object draggedData;
        /// <summary>
        /// Optionally to be set by the drop target. If true, the drop was successful.
        /// This is only really meant to be used between the drop target and drag source during the drop and drag end event.
        /// In the event it was not set, it will default to false.
        /// Will be reset to false when DragStart() is called.
        /// </summary>
        [NonSerialized]
        public bool optionalDropSuccess = false;
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
        /// Starts dragging with the given data. The cursor sprite will be set to the given sprite.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cursorSprite"></param>
        public void StartDrag(object data, Sprite cursorSprite = null)
        {
            optionalDropSuccess = false;
            draggedData = data;
            image.SetSprite(cursorSprite);
        }
        /// <summary>
        /// Ends the dragging. Clears draggedData. The cursor sprite will be set to null.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cursorSprite"></param>
        public void EndDrag()
        {
            draggedData = null;
            image.SetSprite(null);
        }

        public object GetDraggedData() => draggedData;


        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, Input.mousePosition, 0.5f);
        }
    }
}