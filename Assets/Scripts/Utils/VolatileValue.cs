using System;

namespace Utils
{
    /// <summary>
    /// A value that changes, and have events that can be subscribed to.
    /// </summary>
    public class VolatileValue<T>
    {
        public event Action Changed;
        private T _value;

        public T value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke();
            }
        }

        public VolatileValue(T initialValue = default)
        {
            _value = initialValue;
        }
    }
}