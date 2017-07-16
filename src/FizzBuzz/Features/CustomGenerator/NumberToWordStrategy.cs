namespace FizzBuzz.Features.CustomGenerator
{
    public sealed class NumberToWordDivisibleStrategy : IWordStrategy
    {
        private readonly int _divisor;

        private readonly string _word;

        public NumberToWordDivisibleStrategy(int divisor, string word)
        {
            _divisor = divisor;
            _word = word;
        }

        public WordModel Process(WordModel model) => IsDivisible(model) ? model.Add(_word) : model;

        private bool IsDivisible(WordModel model) => model.Number % _divisor == 0;
    }
}
