using System.Collections.Generic;
using System.Linq;

namespace FizzBuzz.Features.CustomGenerator
{
    public class WordGenerator : IWordGenerator
    {
        private readonly IWordStrategy _pipeline;

        public WordGenerator(IWordStrategy pipeline)
        {
            _pipeline = pipeline;
        }

        public IEnumerable<string> GetNumbers(int start, int end)
        {
            return Enumerable.Range(start, end - start + 1).Select(i => _pipeline.Process(new WordModel(i)).ToString());
        }
    }
}