using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    /// <summary>
    /// Represents a button that changes sprite when hovered over or clicked.
    /// This is redundant because the Unity Button component already does this, but I didn't know that at the time.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class SpriteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // Reference to the image component
        public Image image;

        [Tooltip("Sprite to use when button is not hovered over or clicked")]
        public Sprite neutral;

        [Tooltip("Sprite to use when button is hovered over")]
        public Sprite hover;

        [Tooltip("Sprite to use when button is clicked")]
        public Sprite active;
        
        /// <summary>
        /// Set this to true to force the button to be active (display the active sprite)
        /// When overriding, call UpdateImageSprite() to update the sprite
        /// </summary>
        public bool activeOverride = false;

        private bool isHovering;
        private bool isPressed;

        public void UpdateImageSprite()
        {
            // ---- Logic for determining which sprite to use----
            // if mouse is pressed and hovering, use active sprite (Or activeOverride) is true)
            if ((isPressed && isHovering) || activeOverride)
            {
                image.sprite = active;
            }
            // if mouse is hovering, use hover sprite
            else if (isHovering)
            {
                image.sprite = hover;
            }
            // otherwise, use neutral sprite
            else
            {
                image.sprite = neutral;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // When mouse enters this game object, set isHovering to true & update the sprite
            isHovering = true;
            UpdateImageSprite();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // When mouse exits this game object, set isHovering to false & update the sprite
            isHovering = false;
            UpdateImageSprite();
        }


        private void Update()
        {
            
            bool last = isPressed; // If the mouse was pressed last frame
            isPressed = GameManager.Controls.Menus.MouseClick.IsPressed(); // If the mouse is pressed this frame
            // If lastFrame != thisFrame, update the sprite
            if (isPressed != last)
            {
                UpdateImageSprite();
            }
        }
    }
}