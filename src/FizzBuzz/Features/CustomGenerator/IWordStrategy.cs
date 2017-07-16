namespace FizzBuzz.Features.CustomGenerator
{
    public interface IWordStrategy
    {
        WordModel Process(WordModel model);
    }
}