using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Utils
{
    /// <summary>
    /// Linearly interpolates the value of an axis over time. Useful for smoothing out input.
    /// </summary>
#if UNITY_EDITOR // Only add this attribute if we are in the editor
    [InitializeOnLoad]
#endif
    public class ValueLerpProcessor : InputProcessor<float>
    {
#if UNITY_EDITOR // Only initialize the processor in the static constructor if we are in the editor
        static ValueLerpProcessor()
        {
            Initialize();
        }
#endif

        [RuntimeInitializeOnLoadMethod] // Initialize the processor in the runtime
        static void Initialize()
        {
            // Register the processor with the input system
            InputSystem.RegisterProcessor<ValueLerpProcessor>();
        }

        [Tooltip("How long it takes to go from 0 to 1 in seconds (or vice versa)")]
        public float duration = 0.4f;

        // The current value of the lerp
        private float currentValue = 0f;


        public override float Process(float value, InputControl control)
        {
            if (value.IsNegative() != currentValue.IsNegative()) // If going from negative to positive (or vice versa), start smoothing from 0
            {
                currentValue = 0;
            }
            // Linearly interpolate the value over time and clamp it between -1 and 1
            currentValue = Mathf.Clamp(currentValue + (value / duration) * Time.deltaTime, -1f, 1f);
            // Return the current value
            return currentValue;
        }
    }
}