using System;

namespace Core.Utils
{
    /// <summary>
    /// Utility class for storing a range of values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ValueRange <T>
    {
        public T min;
        public T max;

        public ValueRange(T min, T max)
        {
            this.min = min;
            this.max = max;
        }
    }
}