using System.Collections.Generic;

namespace FizzBuzz
{
    public interface IWordGenerator
    {
        IEnumerable<string> GetNumbers(int start, int end);
    }
}