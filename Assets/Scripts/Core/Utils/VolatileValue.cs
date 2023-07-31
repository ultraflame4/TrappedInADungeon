using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Utils
{
    /// <summary>
    /// A value that changes, and have events that can be subscribed to.
    /// </summary>
    [Serializable, JsonObject(MemberSerialization.OptIn)]
    public class VolatileValue<T>
    {
        public event Action Changed;

        /// <summary>
        /// The callback to validate / intercept any value changes. Should return the new value.
        /// </summary>
        public delegate T ValidateValueChange(T oldValue, T newValue);
        [SerializeField,JsonProperty]
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
                }
                else
                {
                    _value = validator.Invoke(_value, value);    
                }
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
        
        public static implicit operator T(VolatileValue<T> volatileValue) => volatileValue.value;
    }
}