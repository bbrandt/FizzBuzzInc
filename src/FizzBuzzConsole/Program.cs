using System;
using FizzBuzz;

namespace FizzBuzzConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            RunWordGenerator();

            //// RunLegacyGenerator();
        }

        private static void RunWordGenerator()
        {
            var wordGenerator = new WordGenerator(new[]
                {new NumberToWordDivisibleStrategy(3, "fizz"), new NumberToWordDivisibleStrategy(5, "buzz")});
        }

        private static void RunLegacyGenerator()
        {
            var fizzBuzzer = new FizzBuzzGenerator();

            foreach (var value in fizzBuzzer.GetNumbers(1, Int32.MaxValue))
            {
                Console.WriteLine(value);
            }
        }
    }
}