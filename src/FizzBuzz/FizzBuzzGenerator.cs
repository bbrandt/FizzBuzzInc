using System.Collections.Generic;
using System.Linq;

namespace FizzBuzz
{
    public class FizzBuzzGenerator
    {
        public IEnumerable<string> GetNumbers(int start, int end)
        {
            foreach (var i in Enumerable.Range(start, end-start+1))
            {
                if (i % 15 == 0)
                {
                    yield return "fizzbuzz";
                }
                else if (i % 3 == 0)
                {
                    yield return "fizz";
                }
                else if (i % 5 == 0)
                {
                    yield return "buzz";
                }
                else
                {
                    yield return i.ToString();
                }
            }
        }

    }
}
