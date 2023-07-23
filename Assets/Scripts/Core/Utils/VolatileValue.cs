using System;
using UnityEngine;

namespace Core.Utils
{
    /// <summary>
    /// A value that changes, and have events that can be subscribed to.
    /// </summary>
    [Serializable]
    public class VolatileValue<T>
    {
        public event Action Changed;

        /// <summary>
        /// The callback to validate the value change.
        /// </summary>
        public delegate T ValidateValueChange(T oldValue, T newValue);
        [SerializeField]
        private T _value;
        public ValidateValueChange validator;
        
        public T value
        {
            get => _value;
            set
            {
                if (validator == null)
                {
                    _value = value;
                    return;
                }
                _value = validator.Invoke(_value, value);
                Changed?.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialValue">Initial value</param>
        /// <param name="validator">A callback to intercept value changes to validate or make the changes to the new value</param>
        public VolatileValue(T initialValue = default, ValidateValueChange validator = null)
        {
            _value = initialValue;
            this.validator = validator;
        }
    }
}