using System;

namespace Core.Utils
{
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