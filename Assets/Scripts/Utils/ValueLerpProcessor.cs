using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class ValueLerpProcessor : InputProcessor<float>
    {
#if UNITY_EDITOR
        static ValueLerpProcessor()
        {
            Initialize();
        }
#endif

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            InputSystem.RegisterProcessor<ValueLerpProcessor>();
        }

        [Tooltip("How long it takes to go from 0 to 1 in seconds (or vice versa)")]
        public float duration = 0.4f;

        private float currentValue = 0f;


        public override float Process(float value, InputControl control)
        {
            if (value.IsNegative() != currentValue.IsNegative()) // If going from negative to positive (or vice versa), start smoothing from 0
            {
                currentValue = 0;
            }

            currentValue = Mathf.Clamp(currentValue + (value / duration) * Time.deltaTime, -1f, 1f);
            
            return currentValue;
        }
    }
}