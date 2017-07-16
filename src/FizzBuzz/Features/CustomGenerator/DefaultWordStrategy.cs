namespace FizzBuzz.Features.CustomGenerator
{
    class DefaultWordStrategy : IWordStrategy
    {
        public WordModel Process(WordModel model)
        {
            return model;
        }
    }
}