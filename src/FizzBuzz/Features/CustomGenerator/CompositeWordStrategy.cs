using System.Collections.Generic;
using System.Linq;

namespace FizzBuzz.Features.CustomGenerator
{
    public class CompositeWordStrategy : IWordStrategy
    {
        private readonly IEnumerable<IWordStrategy> _pipeline;

        public CompositeWordStrategy(IEnumerable<IWordStrategy> pipeline)
        {
            _pipeline = new []{new DefaultWordStrategy()}.Concat(pipeline).ToArray();
        }

        public WordModel Process(WordModel model)
        {
            return _pipeline.Aggregate(model, (currentModel, wordStrategy) => wordStrategy.Process(currentModel));
        }
    }
}