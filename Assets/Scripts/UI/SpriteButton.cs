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
        
        /// <summary>
        /// Set this to true to force the button to be active (display the active sprite)
        /// When overriding call UpdateImageSprite() to update the sprite
        /// </summary>
        public bool activeOverride = false;

        private bool isHovering;
        private bool isPressed;

        public void UpdateImageSprite()
        {
            if ((isPressed && isHovering) || activeOverride)
            {
                image.sprite = active;
            }
            else if (isHovering)
            {
                image.sprite = hover;
            }
            else
            {
                image.sprite = neutral;
            }
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


        private void Update()
        {
            bool last = isPressed;
            isPressed = GameManager.Controls.Menus.MouseClick.IsPressed();
            if (isPressed != last)
            {
                UpdateImageSprite();
            }
        }
    }
}