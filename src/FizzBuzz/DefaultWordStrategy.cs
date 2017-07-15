namespace FizzBuzz
{
    class DefaultWordStrategy : IWordStrategy
    {
        public WordModel Process(WordModel model)
        {
            return model;
        }
    }
}