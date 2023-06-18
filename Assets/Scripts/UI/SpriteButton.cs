using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class SpriteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image image;
        public Sprite neutral;
        public Sprite hover;
        public Sprite active;

        private bool isHovering;
        private bool isPressed;


        private void Update()
        {
            var last = isPressed;
            isPressed = Input.GetKey(KeyCode.Mouse0);
            if (isPressed != last) UpdateImageSprite();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;
            UpdateImageSprite();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
            UpdateImageSprite();
        }

        private void UpdateImageSprite()
        {
            if (isPressed && isHovering)
                image.sprite = active;
            else if (isHovering)
                image.sprite = hover;
            else
                image.sprite = neutral;
        }
    }
}