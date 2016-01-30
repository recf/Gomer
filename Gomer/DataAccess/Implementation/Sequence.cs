using System;

namespace Gomer.DataAccess.Implementation
{
    public class Sequence
    {
        public int StartValue { get; private set; }

        public int Step { get; private set; }

        private int? _currentValue;

        /// <summary>
        /// Gets the current value in the sequence.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when called without first calling <see cref="NextValue"/>
        /// </exception>
        public int CurrentValue()
        {
            // ReSharper disable once PossibleInvalidOperationException
            return _currentValue.Value;
        }

        public Sequence()
            : this(1, 1)
        {
        }

        public Sequence(int startValue)
            : this(startValue, 1)
        {
        }

        public Sequence(int startValue, int step)
        {
            StartValue = startValue;
            Step = step;
        }

        public int NextValue()
        {
            if (!_currentValue.HasValue)
            {
                _currentValue = StartValue;
            }
            else
            {
                _currentValue += Step;
            }

            return _currentValue.Value;
        }
    }
}
