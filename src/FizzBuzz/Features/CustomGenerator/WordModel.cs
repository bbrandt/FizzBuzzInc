using System.Collections.Immutable;

namespace FizzBuzz.Features.CustomGenerator
{
    public sealed class WordModel
    {
        private readonly ImmutableQueue<string> _wordStack;

        public int Number { get; }

        public WordModel(int number)
            :this(number, ImmutableQueue<string>.Empty)
        {
        }

        private WordModel(int number, ImmutableQueue<string> wordStack)
        {
            Number = number;
            _wordStack = wordStack;
        }

        public WordModel Add(string word) => new WordModel(Number, _wordStack.Enqueue(word));

        public override string ToString()
        {
            if (_wordStack.IsEmpty)
            {
                return Number.ToString();
            }

            return string.Join(string.Empty, _wordStack);
        }
    }
}
