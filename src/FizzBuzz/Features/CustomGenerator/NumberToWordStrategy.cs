using System;

namespace FizzBuzz.Features.CustomGenerator
{
    public sealed class NumberToWordDivisibleStrategy : IWordStrategy
    {
        private readonly int _divisor;

        private readonly string _word;

        public NumberToWordDivisibleStrategy(int divisor, string word)
        {
            if (divisor == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(divisor), "A number may not be divided by zero.");
            }

            _divisor = divisor;
            _word = word;
        }

        public WordModel Process(WordModel model) => IsDivisible(model) ? model.Add(_word) : model;

        private bool IsDivisible(WordModel model) => model.Number % _divisor == 0;
    }
}
