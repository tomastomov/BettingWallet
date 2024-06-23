using System;
using System.Collections.Generic;

namespace BettingWallet.Tests
{
    public  class MockRandom : Random
    {
        private readonly Queue<int> _intValues;
        private readonly Queue<double> _doubleValues;

        public MockRandom()
        {
            _intValues = new Queue<int>();
            _doubleValues = new Queue<double>();
        }

        public void AddInt(int value)
        {
            _intValues.Enqueue(value);
        }

        public void AddDouble(double value)
        {
            _doubleValues.Enqueue(value);
        }

        public override int Next(int minValue, int maxValue)
        {
            if (_intValues.Count == 0)
            {
                throw new InvalidOperationException("No int values in queue");
            }

            return _intValues.Dequeue();
        }

        public override double NextDouble()
        {
            if (_doubleValues.Count == 0)
            {
                throw new InvalidOperationException("No double values in queue");
            }

            return _doubleValues.Dequeue();
        }
    }
}
